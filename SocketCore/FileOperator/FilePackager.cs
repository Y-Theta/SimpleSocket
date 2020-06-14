///------------------------------------------------------------------------------
/// @ Y_Theta
///------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Runtime.InteropServices;
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
        private static List<FileQueue> _Queues;

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
        private static void package(string filename, int workingthreads, ref FileQueue queue,ref FilePackageInfo fpinfo, EventHandler<FileQueueArgs> ready, EventHandler<FileQueueArgs> completed) {
            if (!File.Exists(filename)) {
                throw new ArgumentException("File Not Exist !");
            }

            FileInfo info = new FileInfo(filename);
            FilePackageInfo fp = new FilePackageInfo {
                FileName = info.Name,
                FileSize = info.Length,
                ThreadsCounts = workingthreads
            };
            fpinfo = fp;

            // packagesize = all data packages
            FileQueue fq =  new FileQueue(fp, filename);
            queue = fq;
            fq.Ready += ready;
            fq.Progress += completed;
            fq.Pack();

        }


        private static void unpackage(string filename, int workingthreads, ref FileQueue queue, ref FilePackageInfo fpinfo, EventHandler<FileQueueArgs> ready, EventHandler<FileQueueArgs> completed) {

        }


        /// <summary>
        /// 将文件分包
        /// </summary>
        /// <param name="filename">要分包的文件名称</param>
        /// <param name="workingthreads">使用的线程数,产生同数量的包队列</param>
        /// <param name="queue">文件包队列实例</param>
        /// <param name="OnReady"><打包准备完成时触发/param>
        /// <param name="OnProgress">打包过程中触发</param>
        /// <returns></returns>
        public void Package(
            string filename, 
            int workingthreads, 
            ref FileQueue queue, 
            ref FilePackageInfo fpinfo,
            EventHandler<FileQueueArgs> OnReady, 
            EventHandler<FileQueueArgs> OnProgress) {

            package(filename, workingthreads, ref queue,ref fpinfo, OnReady, OnProgress);
        }

        /// <summary>
        /// 组装文件包
        /// </summary>
        /// <param name="info"></param>
        public void UnPackage(string filename, FilePackageInfo info, Action<double> Prograss) {

        }


        /// <summary>
        /// 获得所有的包队列
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, FileQueue> GetQueues() {
            return null;

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
            _Queues = new List<FileQueue>();
        }

        #endregion
    }
}
