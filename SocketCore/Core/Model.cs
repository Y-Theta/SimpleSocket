///------------------------------------------------------------------------------
/// @ Y_Theta
///------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SocketCore.Core {

    /// <summary>
    /// 接收数据
    /// </summary>
    /// <returns></returns>
    public delegate byte[] ByteDataReceiveHandle();
    public delegate void ByteDataSendHandle(byte[] data);
    /// <summary>
    /// 接收数据处理
    /// Received Byte Resovle Handle
    /// </summary>
    /// <param name="data"></param>
    public delegate void RecDataResHandle(byte[] data);

    public enum SocketRole {
        Server,
        Client
    }
}
