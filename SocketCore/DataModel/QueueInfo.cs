using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SocketCore.DataModel {

    /// <summary>
    /// 队列信息
    /// </summary>
    [StructLayout(LayoutKind.Explicit,Pack = 4)]
    public struct QueueInfo {

        /// <summary>
        /// 队列中的包的相对整个文件的起始偏移
        /// </summary>
        [FieldOffset(0)]
        public long Offset;

        /// <summary>
        /// 队列需要打包或解包的字节总数
        /// </summary>
        [FieldOffset(8)]

        public long Length;

        /// <summary>
        /// 队列总共需要打的包数
        /// </summary>
        [FieldOffset(16)]

        public int PackCount;
    }
}
