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


namespace SocketCore.Core {

    public class SocketBase : ISocketMessageContract {
        #region Properties
        public IPAddress Address { get; set; }

        public int Port { get; set; }

        public bool Actived { get; set; }

        /// <summary>
        /// 关联此事件以进行接收监听
        /// </summary>
        protected PackageResolveHandle _onmessageReceived;
        public event PackageResolveHandle OnMessageReceive {
            add => _onmessageReceived += value;
            remove => _onmessageReceived -= value;
        }

        #endregion

        #region Methods

        #region ISocketMsgContract
        public void Init() {
            Socket s;
        }

        public void Dispose() {
            
        }

        #endregion

        #endregion

        #region Constructors
        #endregion


    }
}
