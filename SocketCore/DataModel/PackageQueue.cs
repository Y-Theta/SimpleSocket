///------------------------------------------------------------------------------
/// @ Y_Theta
///------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using SocketCore.Extension;

namespace SocketCore.DataModel {

    /// <summary>
    /// 数据包队列
    /// 对应于每个UDP socket
    /// </summary>
    public class PackageQueue : IDisposable {
        #region Properties

        /// <summary>
        /// 包队列暂存的包数量
        /// 超过此数量时文件打包器不再继续产生包
        /// </summary>
        private static int _maxcache = 128;

        private static HashSet<string> _QueueStatus;

        /// <summary>
        /// 
        /// </summary>
        private MemoryMappedFile _filecache;

        /// <summary>
        /// 
        /// </summary>
        private Queue<UserPackage> _packages;

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
        /// 此队列产生的包在源文件中的起始偏移
        /// </summary>
        public double Offset;

        /// <summary>
        /// 此队列应产生的包的总数量
        /// </summary>
        public int Length;

        /// <summary>
        /// 包大小
        /// </summary>
        public int PackageSize;

        /// <summary>
        /// 当前在自身所读文件块中的偏移
        /// </summary>
        private long _currentoffset;

        /// <summary>
        /// 当前包的序号
        /// </summary>
        private int _currentpackageid;

        /// <summary>
        /// 指示每个视图的大小
        /// </summary>
        private long _partsize;

        private long PartSize {
            get {
                if (_partsize <= 100) {
                    long fulllength = Length * PackageSize;
                    long cachesize = PackageSize * _maxcache;
                    _partsize = Math.Min(fulllength, cachesize);
                }
                return _partsize;
            }
        }

        /// <summary>
        /// 暂存当前用于读写文件的内存视图
        /// </summary>
        private MemoryMappedViewAccessor _accessor;
        private bool disposedValue;


        #endregion

        #region Methods

        /// <summary>
        /// 开始打包线程
        /// </summary>
        private void pack() {
            Task.Run(() => {
                try {
                    _accessor = _filecache.CreateViewAccessor(_currentoffset, _partsize);
                } catch {

                }
                long tempoffset = 0;
                for (; _currentpackageid < Length;) {
                    byte[] buffer = new byte[PackageSize];
                    _accessor.ReadArray(tempoffset, buffer, 0, PackageSize);

                    //打包
                    UserPackage up = buffer.Package(pid: _currentpackageid, poffset: _currentoffset);

                    _currentoffset += PackageSize;
                    tempoffset += PackageSize;
                    _currentpackageid++;

                    //移动映射文件访问器位置
                    if (tempoffset >= _partsize) {
                        //打包完成
                        if (_currentpackageid + 1 >= Length) {
                            _accessor.Dispose();
                            _accessor = null;
                            break;
                        }
                        _accessor.Dispose();
                        _accessor = null;

                        long lengthremain = PackageSize * (Length - _currentpackageid);
                        long newsize = Math.Min(lengthremain, _partsize);
                        try {
                            _accessor = _filecache.CreateViewAccessor(_currentoffset, newsize);
                        } catch {

                        }
                        tempoffset = 0;

                    }

                    //当打的包没有被取走时，停止打包
                    while (!Add(up)) { Thread.Sleep(5); };
                }
            });
        }

        public bool Busy() {
            return _QueueStatus.Contains(_id);
        }

        /// <summary>
        /// 向队列中添加数据包
        /// </summary>
        /// <param name="pack"></param>
        /// <returns></returns>
        public bool Add(UserPackage pack) {
            if (_packages.Count < PackageQueue._maxcache) {
                lock (_packages) {
                    if (_packages.Count < PackageQueue._maxcache) {
                        _packages.Enqueue(pack);
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 从队列中获取数据包
        /// </summary>
        /// <returns></returns>
        public UserPackage Get() {
            UserPackage p;
            lock (_packages) {
                p = _packages.Dequeue();
            }
            return p;
        }
        #endregion

        #region Constructors
        static PackageQueue() {
            _QueueStatus = new HashSet<string>();
        }

        public PackageQueue(ref MemoryMappedFile file) {
            _filecache = file;
            _packages = new Queue<UserPackage>(16);
            _currentoffset = 0;
            _currentpackageid = 0;
            _QueueStatus.Add(ID);
        }

        protected virtual void Dispose(bool disposing) {
            if (!disposedValue) {
                if (disposing) {
                    if (_accessor != null) {
                        _accessor.Dispose();
                        _accessor = null;
                    }

                    _packages.Clear();
                    _packages = null;
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
