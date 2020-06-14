///------------------------------------------------------------------------------
/// @ Y_Theta
///------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using SocketCore.DataModel;
using SocketCore.Extension;

namespace SocketCore.FileOperator {

    /// <summary>
    /// 数据包队列 对应于每个待打包文件
    /// </summary>
    public class FileQueue : IDisposable {
        #region Properties

        /// <summary>
        /// 包队列暂存的包数量
        /// 超过此数量时文件打包器不再继续产生包
        /// </summary>
        private int _maxcache = 256;
        /// <summary>
        /// 32 + 1440 = 1472 
        /// 最长不会被分IP包的长度
        /// </summary>
        private int _packsize = 1440;


        private static Dictionary<QueueInfo, Task> _packageTasks;

        /// <summary>
        /// 此队列标识
        /// </summary>
        private string _id;
        public string ID {
            get {
                if (string.IsNullOrEmpty(_id)) {
                    _id = Guid.NewGuid().ToString("D");
                }
                return _id;
            }
        }

        /// <summary>
        /// 暂存当前用于读写文件的内存视图
        /// </summary>
        private bool disposedValue;

        /// <summary>
        /// 文件在当前系统的全路径
        /// </summary>
        private string _filefullpath;
        private MemoryMappedFile _filecache;

        /// <summary>
        /// 当前队列对应的文件信息
        /// </summary>
        public FilePackageInfo Info;

        /// <summary>
        /// 包队列
        /// </summary>
        public List<Queue<UserPackage>> PackageQueues { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public List<QueueInfo> Queueinfos { get; private set; }

        /// <summary>
        /// 进度汇报的触发灵敏度 
        /// <para><see cref="Progress"/> will Invoke per <see cref="ProgressSensitive"/> packages</para>
        /// </summary>
        public int ProgressSensitive { get; private set; }

        /// <summary>
        /// 当缓存包准备好时触发此事件
        /// </summary>
        public event EventHandler<FileQueueArgs> Ready;

        /// <summary>
        /// 当包队列的状态发生变化时触发此事件
        /// </summary>
        public event EventHandler<FileQueueArgs> Progress;
        #endregion

        #region Methods

        /// <summary>
        /// 根据文件大小以及所用线程数分配包大小及最大包缓存
        /// </summary>
        /// <param name="instance"></param>
        private static void prearrange(FileQueue instance) {

            FilePackageInfo info = instance.Info;
            
            if(info.FileSize < 1 << 25) {
                // less than 32MB
                instance._maxcache = 256;
                instance._packsize = 1440;
            }else if ((info.FileSize >= 1 << 25 )&& (info.FileSize < 1 << 30)) {
                // 32 - 1024 MB
                instance._maxcache = 128;
                //UserPackage has a 32 bit control data 
                instance._packsize = 14720 - 32;
            } else {
                // more than 1GB
                instance._maxcache = 256;
                //UserPackage has a 32 bit control data 
                instance._packsize = 14720 - 32;
            }

        }

        /// <summary>
        /// 根据文件信息生成队列信息
        /// </summary>
        /// <param name="info"></param>
        private static QueueInfo[] generatequeueinfo(ref FilePackageInfo info, FileQueue instance) {

            QueueInfo[] infos = new QueueInfo[info.ThreadsCounts];
            var FilePackages = (long)Math.Round((double)(info.FileSize / instance._packsize)) + 1;
            int queuelength = (int)Math.Round((double)(FilePackages / info.ThreadsCounts)) + 1;

            for (int i = 0; i < info.ThreadsCounts; i++) {
                QueueInfo qinfo = new QueueInfo();
                if (i == info.ThreadsCounts - 1) {
                    qinfo.PackCount = (int)(FilePackages - i * queuelength);
                    qinfo.Offset = i * queuelength * instance._packsize;
                    qinfo.Length = info.FileSize - qinfo.Offset;
                } else {
                    qinfo.PackCount = queuelength;
                    qinfo.Offset = i * queuelength * instance._packsize;
                    qinfo.Length = qinfo.PackCount * instance._packsize;
                }
                infos[i] = qinfo;
            }
            return infos;
        }


        /// <summary>
        /// 开始打包线程
        /// </summary>
        private static void pack(FileQueue instance, CancellationTokenSource cancle = null) {
            FilePackageInfo info = instance.Info;
            MemoryMappedFile file = MemoryMappedFile.CreateFromFile(instance._filefullpath, FileMode.Open, info.FileName + "cached", 0, MemoryMappedFileAccess.ReadExecute);
            instance._filecache = file;
            instance.Queueinfos.AddRange(generatequeueinfo(ref info,instance));

            for (int threads = instance.PackageQueues.Count; threads < info.ThreadsCounts; threads++) {
                packqueue(threads, instance, file);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="queueinfo"></param>
        /// <param name="filecache"></param>
        private static void packqueue(int id, FileQueue instance, MemoryMappedFile filecache) {
            instance.PackageQueues.Add(new Queue<UserPackage>());
            Task t = new Task(() => {
                bool first = true;
                QueueInfo info = instance.Queueinfos[id];
                long partoffset = info.Offset;
                long currentoffset = 0;
                int packed = 0;
                long cachemax = instance._packsize * instance._maxcache;
                long pagesize = Math.Min(cachemax, info.Length + info.Offset - partoffset);
                var accessor = filecache.CreateViewAccessor(partoffset, pagesize, MemoryMappedFileAccess.Read);
                for (; packed < info.PackCount;) {
                    byte[] buffer = new byte[instance._packsize];
                    int actrualsize = accessor.ReadArray(currentoffset, buffer, 0, instance._packsize);
                    UserPackage up = buffer.Package(pid: packed, poffset: partoffset - info.Offset);
                    up.Reserved = id;
                    packed++;
                    currentoffset += instance._packsize;
                    partoffset += instance._packsize;

                    if (packed % instance.ProgressSensitive == 0) {
                        instance.Progress?.Invoke(instance, new FileQueueArgs(id, info, new QueueInfo {
                            Offset = info.Offset,
                            Length = partoffset - info.Offset,
                            PackCount = packed
                        }));
                    }

                    if (currentoffset >= pagesize) {
                        if (packed + 1 >= info.PackCount) {
                            accessor.Dispose();
                            accessor = null;
                        }

                        currentoffset = 0;
                        pagesize = Math.Min(cachemax, info.Length + info.Offset - partoffset);
                        accessor = filecache.CreateViewAccessor(partoffset, pagesize, MemoryMappedFileAccess.Read);
                    }

                    //当打的包没有被取走时，停止打包
                    while (!instance.Add(id, up)) {
                        Thread.Sleep(5);
                        if (first) {
                            first = false;
                            instance.Ready?.Invoke(instance, new FileQueueArgs(id, info, info));
                        }
                    }
                }
            });
            t.Start();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="instance"></param>
        private static void unpack(FileQueue instance) {
            FilePackageInfo info = instance.Info;
            MemoryMappedFile file = MemoryMappedFile.CreateFromFile(instance._filefullpath, FileMode.OpenOrCreate, info.FileName + "cached", info.FileSize, MemoryMappedFileAccess.ReadWrite);
            instance._filecache = file;
            instance.Queueinfos.AddRange(generatequeueinfo(ref info, instance));

            for (int threads = instance.PackageQueues.Count; threads < info.ThreadsCounts; threads++) {
                unpackqueue(threads, instance, file);
            }
        }

        /// <summary>
        /// 解包队列
        /// </summary>
        /// <param name="id"></param>
        /// <param name="queueinfo"></param>
        /// <param name="filecache"></param>
        private static void unpackqueue(int id, FileQueue queueinfo, MemoryMappedFile filecache) {
            queueinfo.PackageQueues.Add(new Queue<UserPackage>());
            Task t = new Task(() => {
                QueueInfo info = queueinfo.Queueinfos[id];
                int unpacked = 0;

            });
            t.Start();
        }

        public void Pack() {
            pack(this);
        }

        /// <summary>
        /// 向队列中添加数据包
        /// </summary>
        /// <param name="pack"></param>
        /// <returns></returns>
        public bool Add(int ID, UserPackage pack) {
            Queue<UserPackage> taskupq = PackageQueues[ID];
            if (taskupq.Count < _maxcache)
                lock (taskupq) {
                    if (taskupq.Count < _maxcache) {
                        taskupq.Enqueue(pack);
                        return true;
                    }
                }
            return false;
        }

        public bool Add(int ID, UserPackage[] packs) {
            Queue<UserPackage> taskupq = PackageQueues[ID];
            if (taskupq.Count + packs.Length < _maxcache)
                lock (taskupq) {
                    if (taskupq.Count + packs.Length < _maxcache) {
                        foreach (UserPackage p in packs) {
                            taskupq.Enqueue(p);
                        }
                        return true;
                    }
                }
            return false;
        }

        /// <summary>
        /// 从队列中获取数据包
        /// </summary>
        /// <returns></returns>
        public bool Get(int ID, out UserPackage? p) {
            Queue<UserPackage> taskupq = PackageQueues[ID];
            p = null;
            lock (taskupq) {
                if (taskupq.Count > 0) {
                    p = taskupq.Dequeue();
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region Constructors
        static FileQueue() {
            _packageTasks = new Dictionary<QueueInfo, Task>();
        }

        public FileQueue(FilePackageInfo info, string fullpath) : this(info) {
            _filefullpath = fullpath;
        }

        public FileQueue(FilePackageInfo info) {
            PackageQueues = new List<Queue<UserPackage>>();
            Queueinfos = new List<QueueInfo>();
            ProgressSensitive = _maxcache;
            Info = info;
        }

        protected virtual void Dispose(bool disposing) {
            if (!disposedValue) {
                if (disposing) {
                    _filecache.Dispose();
                }
                // TODO: 释放未托管的资源(未托管的对象)并替代终结器
                // TODO: 将大型字段设置为 null
                disposedValue = true;
            }
        }

        public void Dispose() {
            // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
