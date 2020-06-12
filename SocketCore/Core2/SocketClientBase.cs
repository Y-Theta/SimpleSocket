using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace SocketCore.Core {
    public class SocketClientBase : SocketBase, ISocketClient {

        Socket _client;

        private ProtocolType _protocolType;
        /// <summary>
        /// 服务端的通信模式
        /// </summary>
        public new ProtocolType ProtocolType {
            get => _protocolType;
            set => switchProtocolType(value);
        }


        public new bool Actived { get => _client == null ? false : _client.Connected; }

        public override void Init() {
            if (_protocolType == ProtocolType.Unknown || _protocolType == ProtocolType.Unspecified)
                _protocolType = ProtocolType.Tcp;
            switchProtocolType(ProtocolType);
        }

        protected override void switchProtocolType(ProtocolType type) {
            if (Selector == null)
                Selector = new SocketTypeSelector();
            _protocolType = type;
            if (type == ProtocolType.Unknown || type == ProtocolType.Unspecified)
                _protocolType = ProtocolType.Tcp;
            Selector.SwitchProtocolType(ref _client, type, null);
        }

        public void BeginConnent(int timeout) {
            var e = new SocketAsyncEventArgs {
                RemoteEndPoint = new IPEndPoint(Address, Port),
            };
            e.Completed += E_Completed;

            _client.ConnectAsync(e);

        }

        private void E_Completed(object sender, SocketAsyncEventArgs e) {
            if (e.SocketError == SocketError.Success) {
                var arg = new SocketAsyncEventArgs();
                arg.Completed += Arg_Completed;
                arg.UserToken = _client;
                arg.SocketFlags = SocketFlags.None;
                arg.SetBuffer(new byte[1024], 0, 1024);
                _client.ReceiveAsync(arg);
            }
            _client.ConnectAsync(e);
        }

        private void Arg_Completed(object sender, SocketAsyncEventArgs e) {
            Socket s = e.UserToken as Socket;
            if (e.SocketError == SocketError.Success) {
                if (e.Buffer.UnPackage() is Package p)
                    invokeReceive(p);
            }
            var arg = new SocketAsyncEventArgs();
            arg.Completed += Arg_Completed;
            arg.UserToken = s;
            arg.SetBuffer(e.Buffer, 0, e.Buffer.Length);
            e.Dispose();
            s.ReceiveAsync(arg);

        }

        private void SocketClientBase_Completed(object sender, SocketAsyncEventArgs e) {

        }

        public override void Dispose() {

        }

        public void Send(byte[] data) {
            var p = (Package)data.Package();
            _client.Send(p.Serialize(), SocketFlags.None);
        }

        public void SendAsync(byte[] data, Action ondatasended) {

        }


    }
}
