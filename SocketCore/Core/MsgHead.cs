///------------------------------------------------------------------------------
/// @ Y_Theta
///------------------------------------------------------------------------------
using System;
using System.Runtime.InteropServices;


namespace SocketCore.Core {

    /// <summary>
    /// 用于识别包的数据头
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Size = 8)]
    public struct MsgHead : IPackageMessage {
        #region Properties
        [FieldOffset(0)]
        private Int32 _packid;

        [FieldOffset(4)]
        private UInt32 _packsize;

        /// <summary>
        /// 包序号
        /// </summary>
        public int PACKID {
            get => _packid;
            set => _packid = value;
        }

        /// <summary>
        /// 包大小
        /// </summary>
        public UInt32 PACKSIZE {
            get => _packsize;
            set => _packsize = value;
        }
        #endregion
    }
}
