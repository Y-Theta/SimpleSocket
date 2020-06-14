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
    /// 
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    public class UserPackage {
        #region Properties

        /// <summary>
        /// 客户端ID
        /// </summary>
        [FieldOffset(0)]
        public Int32 CID;

        /// <summary>
        /// 包序号
        /// </summary>
        [FieldOffset(4)]
        public Int32 PACKID;

        /// <summary>
        /// 当前包内容在文件中的偏移量
        /// </summary>
        [FieldOffset(8)]
        public long PACKOFFSET;

        /// <summary>
        /// 包大小
        /// </summary>
        [FieldOffset(16)]
        public Int32 PACKSIZE;

        /// <summary>
        /// 包发出时间戳
        /// </summary>
        [FieldOffset(20)]
        public double TimeStamp;

        /// <summary>
        /// 保留字段
        /// </summary>
        [FieldOffset(28)]
        public Int32 Reserved;

        /// <summary>
        /// 数据字段
        /// </summary>
        [FieldOffset(32)]
        public byte[] DATA;
        #endregion

        #region Methods
        public void Dispose() {
            PACKSIZE = 0;
            DATA = null;
        }
        #endregion

        #region Constructors
        #endregion
    }
}
