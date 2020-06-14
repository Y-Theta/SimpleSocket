///------------------------------------------------------------------------------
/// @ Y_Theta
///------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;


namespace SocketCore.DataModel {

    /// <summary>
    /// 用于网络发送时对文件基本信息的包装
    /// 作为第一个数据包发送
    /// 使接收端做好响应文件接收的准备
    /// </summary>
    [StructLayout(LayoutKind.Explicit, CharSet = CharSet.Unicode,Pack = 4)]
    public struct FilePackageInfo {
        #region Properties
        /// <summary>
        /// 文件大小 in byte
        /// </summary>
        [FieldOffset(0)]
        public long FileSize;

        /// <summary>
        /// 打包文件所用线程数
        /// 也是UDP发送线程数
        /// </summary>
        [FieldOffset(16)]
        public Int32 ThreadsCounts;

        /// <summary>
        /// 文件名称 128位
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        [FieldOffset(20)]
        public string FileName;
        #endregion

        #region Methods
        #endregion

        #region Constructors
        #endregion
    }
}
