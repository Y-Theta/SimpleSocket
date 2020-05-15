using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketCore.Core {

    /// <summary>
    /// 
    /// </summary>
    interface ISocketClient {

        /// <summary>
        /// 发送信息
        /// </summary>
        /// <param name="data">待发送的数据</param>
        void Send(byte[] data);
        void SendAsync(byte[] data, ByteDataSendHandle ondatasended);

        /// <summary>
        /// 接收信息
        /// </summary>
        /// <returns></returns>
        byte[] Recieive();
        void RecieiveAsync(RecDataResHandle ondatareceived);
    }
}
