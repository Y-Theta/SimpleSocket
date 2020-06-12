using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketCore.Core {

    /// <summary>
    /// 
    /// </summary>
    public interface ISocketClient {

        /// <summary>
        /// 尝试连接客户端
        /// </summary>
        /// <param name="timeout"></param>
        void BeginConnent(int timeout);

        /// <summary>
        /// 发送信息
        /// </summary>
        /// <param name="data">待发送的数据</param>
        void Send(byte[] data);
        void SendAsync(byte[] data, Action ondatasended);
    }
}
