///------------------------------------------------------------------------------
/// @ Y_Theta
///------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace SocketCore.Core {

    /// <summary>
    /// 
    /// </summary>
    public interface ISocketServer {
        #region Properties
        /// <summary>
        /// 客户端
        /// </summary>
        List<Socket> Clients { get; set; }

        /// <summary>
        /// 用于心跳检测的计时器
        /// </summary>
        Dictionary<Socket, Timer> ClientsTimers { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// 发送信息
        /// </summary>
        /// <param name="data">待发送的数据</param>
        void Send(int clientid, byte[] data);
        void SendAsync(int clientid, byte[] data, Action ondatasended);
        #endregion

        #region Constructors
        #endregion
    }
}
