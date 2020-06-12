///------------------------------------------------------------------------------
/// @ Y_Theta
///------------------------------------------------------------------------------
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
    /// <typeparam name="T"></typeparam>
    public delegate void EventHandler<in U>(object sender, U Args);

    /// <summary>
    /// 比特流数据解析委托
    /// </summary>
    /// <param name="data"></param>

    public delegate void PackageResolveHandle(Package package);

    /// <summary>
    /// Scoket接收委托
    /// </summary>
    /// <param name="client"></param>
    /// <param name="accept"></param>
    public delegate void ClientAcceptedHandle(Client client, bool accept = true);


    /// <summary>
    /// 
    /// </summary>
    public class SocketMessageArgs : EventArgs {

    }
}
