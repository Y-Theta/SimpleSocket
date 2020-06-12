///------------------------------------------------------------------------------
/// @ Y_Theta
///------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SocketCore.DataModel;

namespace SocketCore.FileOperator {

    /// <summary>
    /// 
    /// </summary>
    public class FilePackager : IFilePackager, IDisposable {
        #region Properties

        /// <summary>
        /// 压缩器
        /// </summary>
        public ICompress Compresser;

        /// <summary>
        /// 预先准备的包的数量，文件剩余部分在发送过程中再打包
        /// </summary>
        public int PrepareSize;


        private int _workingthreads;
        /// <summary>
        /// 用于打包的线程数
        /// </summary>
        public int WorkingThreads {
            get {
                if (_workingthreads <= 0) {
                    _workingthreads = 1;
                }
                return _workingthreads;
            }

            set {
                _workingthreads = value <= 0 ? _workingthreads == 0 ? 1 : _workingthreads : value;

            }
        }

        /// <summary>
        /// 静态的打包序列字典
        /// </summary>
        private static List<PackageQueue> _Queues;

        /// <summary>
        /// 
        /// </summary>
        private MemoryMappedFile _filecache;

        /// <summary>
        /// 
        /// </summary>
        private bool disposedValue;
        #endregion

        #region Methods


        /// <summary>
        /// 分包文件
        /// </summary>
        /// <param name="filename"></param>
        private static FilePackageInfo package(string filename, int packagesize, int workingthreads, ref MemoryMappedFile insstancecache) {
            if (!File.Exists(filename)) {
                throw new ArgumentException("File Not Exist !");
            }

            FileInfo info = new FileInfo(filename);
            FilePackageInfo fp = new FilePackageInfo {
                FileName = info.Name,
                FileSize = info.Length
            };

            if (packagesize <= 6000)
                throw new ArgumentException("Package length too short");

            // packagesize = all data packages
            fp.FilePackages = Math.Round(fp.FileSize / packagesize, MidpointRounding.AwayFromZero);

            MemoryMappedFile file = MemoryMappedFile.CreateFromFile(filename, FileMode.Open, fp.FileName + "cached", 0, MemoryMappedFileAccess.ReadExecute);
            insstancecache = file;

            while (fp.FilePackages / workingthreads > int.MaxValue)
                workingthreads += 2;

            fp.ThreadsCounts = workingthreads;

            int queuelength = (int)Math.Round(fp.FilePackages / workingthreads, MidpointRounding.AwayFromZero);

            for (int i = 0; i < workingthreads; i++) {
                PackageQueue queue = new PackageQueue(ref file);
                if (i == workingthreads - 1) {
                    queue.Length = (int)(fp.FilePackages - i * queuelength);
                } else {
                    queue.Length = queuelength;
                }
                queue.Offset = i * queue.Length * packagesize;
                queue.PackageSize = packagesize;

                _Queues.Add(queue);
            }

            return fp;
        }

        /// <summary>
        /// 将文件分包
        /// </summary>
        /// <param name="filename">文件绝对名称</param>
        /// <param name="packagesize">所要分成的每个包的大小(/byte)</param>
        public FilePackageInfo Package(string filename, int packagesize) {
            return package(filename, packagesize, WorkingThreads, ref _filecache);
        }


        //public async Task PackageAsync(string filename, int packagesize, Action<FilePackageInfo> callback = null) {
        //    await Task.Run(() => {
        //        FilePackageInfo fp = Package(filename, packagesize);
        //        callback?.Invoke(fp);
        //    });
        //}

        #endregion

        #region Dispose

        protected virtual void Dispose(bool disposing) {
            if (!disposedValue) {
                if (disposing) {
                    // TODO: 释放托管状态(托管对象)
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

        #region Constructors
        static FilePackager() {
            _Queues = new List<PackageQueue>();
        }

        #endregion
    }
}
