///------------------------------------------------------------------------------
/// @ Y_Theta
///------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

using SocketCore.DataModel;
using SocketCore.Extension;

namespace SocketCore.Core {
    public class TCPServer : TCPSocket {

        #region Properties

        private Socket _server;

        /// <summary>
        /// 用来标识此socket接收从哪个IP发往本机哪个端口的消息
        /// </summary>
        public IPEndPoint Endpoint { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Dictionary<IPEndPoint, Client> _clients;

        private List<string> _packagesendarr;
        #endregion

        #region Methods


        public void Send(byte[] data, IPEndPoint to = null) {
            if (to != null) {
                if (!_clients.ContainsKey(to))
                    throw new ArgumentException(string.Format("Client at {0} not Connected ! ", to));
                UserPackage p = data.Package();
                

            } else throw new ArgumentException("IP endPoint not define ! ");
        }
        #endregion

        #region Constructors
        #endregion
    }
}
