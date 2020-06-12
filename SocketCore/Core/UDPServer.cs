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
    public class UDPServer {

        Socket _sock;


        public void Init() {
            _sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Tcp);
        
            
        }
    }
}
