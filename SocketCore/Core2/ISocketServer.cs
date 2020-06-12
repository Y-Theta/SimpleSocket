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
        IList<Client> Clients { get; }

        /// <summary>
        /// 用于心跳检测的计时器
        /// </summary>
        IDictionary<String, Timer> ClientsTimers { get; }

        /// <summary>
        /// 
        /// </summary>
        int ActiveTestInterval { get; set; }

        /// <summary>
        /// 
        /// </summary>
        int MaxConnection { get; set; }

        /// <summary>
        /// 当接收到新客户端时
        /// </summary>
        event ClientAcceptedHandle OnClientAccepted;
        #endregion

        #region Methods
        /// <summary>
        /// 开始接收客户端
        /// </summary>
        void BeginAccept();

        /// <summary>
        /// 发送信息
        /// </summary>
        /// <param name="data">待发送的数据</param>
        void Send(string clientid, byte[] data);
        void SendAsync(string clientid, byte[] data, Action ondatasended);
        #endregion

        #region Constructors
        #endregion
    }
}
