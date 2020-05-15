///------------------------------------------------------------------------------
/// @ Y_Theta
///------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;


namespace SocketCore.Core {
    public class SocketBase : ISocketMsgContract {
        #region Properties
        public IPAddress Address { get; set; }

        public int Port { get; set; }

        public bool Actived { get; set; }

        /// <summary>
        /// 关联此事件以进行接收监听
        /// </summary>
        protected RecDataResHandle _onmessageReceived;
        public event RecDataResHandle OnMessageReceived {
            add => _onmessageReceived += value;
            remove => _onmessageReceived -= value;
        }

        private

        #endregion

        #region Methods

        #region ISocketMsgContract


        public void DisConnect(bool reuse) {

            throw new NotImplementedException();
        }

        public void DisConnectAsync(bool reuse, AsyncCallback ondisconnected) {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 同步接受一条数据
        /// </summary>
        /// <returns></returns>
        public byte[] Recieive() {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 异步接收一条数据
        /// </summary>
        /// <param name="ondatareceived">异步接收回调</param>
        public void RecieiveAsync(RecDataResHandle ondatareceived) {
            ByteDataReceiveHandle func = Recieive;
            func.BeginInvoke(ar => {
                if (ar.IsCompleted)
                    ondatareceived.Invoke((ar.AsyncState as ByteDataReceiveHandle).EndInvoke(ar));
            }, func);
        }

        public void Send(byte[] data) {
            throw new NotImplementedException();
        }

        public void SendAsync(byte[] data, AsyncCallback ondatasended) {
            throw new NotImplementedException();
        }
        #endregion

        #endregion

        #region Constructors
        #endregion


    }
}
