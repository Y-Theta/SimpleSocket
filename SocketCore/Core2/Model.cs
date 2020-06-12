///------------------------------------------------------------------------------
/// @ Y_Theta
///------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;


namespace SocketCore.Core {

    /// <summary>
    /// Socket角色
    /// </summary>
    public enum SocketRole {
        Server,
        Client
    }

    /// <summary>
    /// 数据包
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    public struct Package {
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
        /// 保留字段
        /// </summary>
        [FieldOffset(20)]
        public long Reserved;

        /// <summary>
        /// 数据字段
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray)]
        [FieldOffset(32)]
        public byte[] DATA;
    }

    /// <summary>
    /// 服务器侧客户端实例
    /// </summary>
    public class Client {
        /// <summary>
        /// 
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Socket Socket { get; set; }
    }
}
