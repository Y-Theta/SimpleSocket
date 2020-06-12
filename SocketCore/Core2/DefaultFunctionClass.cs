using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SocketCore.Core {

    /// <summary>
    /// 
    /// </summary>
    public class SocketTypeSelector : ISocketTypeSelector {

        public void SwitchProtocolType(ref Socket socket, ProtocolType protocoltype, Action<ProtocolType> action) {
            if (socket!= null && socket.Connected) {
                socket.Close();
                socket.Disconnect(false);
                socket.Dispose();
                socket = null;
            }
            switch (protocoltype) {
                case ProtocolType.Tcp:
                    socket = new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);
                    break;
                case ProtocolType.Udp:
                    socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                    break;
                default:
                    socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    break;
            }
            action?.Invoke(protocoltype);
        }
    }

}
