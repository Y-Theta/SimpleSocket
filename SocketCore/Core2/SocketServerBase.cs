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

    /// <summary>
    /// 服务端的Socket
    /// </summary>
    public class SocketServerBase : SocketBase, ISocketServer {

        #region Properties
        private Socket _server;

        private ProtocolType _protocolType;
        /// <summary>
        /// 服务端的通信模式
        /// </summary>
        public new ProtocolType ProtocolType {
            get => _protocolType;
            set => switchProtocolType(value);
        }

        private List<Client> _clients;
        /// <summary>
        /// 服务端的所有客户端连接
        /// </summary>
        public IList<Client> Clients {
            get => _clients;
        }

        /// <summary>
        /// 
        /// </summary>
        private HashSet<string> _activeClients;


        private Dictionary<string, Timer> _clientsTimers;
        /// <summary>
        /// 服务端客户端链接的心跳检测计数器
        /// </summary>
        public IDictionary<string, Timer> ClientsTimers {
            get => _clientsTimers;
        }


        private int _activeTestInterval;
        /// <summary>
        /// 心跳检查间隔
        /// </summary>
        public int ActiveTestInterval {
            get => _activeTestInterval;
            set => _activeTestInterval = value <= 0 ? 10 * 1000 : value;
        }

        private int _maxConnection;
        /// <summary>
        /// 最大连接数
        /// </summary>
        public int MaxConnection {
            get => _maxConnection;
            set => _maxConnection = value <= 0 ? 100 : value;
        }

        private ClientAcceptedHandle _onClientAccepted;
        /// <summary>
        /// <inheritdoc/> 此事件触发 
        /// </summary>
        public event ClientAcceptedHandle OnClientAccepted {
            add => _onClientAccepted += value;
            remove => _onClientAccepted -= value;
        }

        public new bool Actived { get => _server == null ? false : _server.Connected; }

        #endregion


        #region Overrides
        /// <summary>
        /// <inheritdoc/> （服务端）
        /// </summary>
        public override void Init() {
            _clients = new List<Client>(16);
            _clientsTimers = new Dictionary<string, Timer>(16);
            _activeClients = new HashSet<string>();

            switchProtocolType(_protocolType);
        }


        protected override void switchProtocolType(ProtocolType type) {
            if (Selector == null)
                Selector = new SocketTypeSelector();
            _protocolType = type;
            if (type == ProtocolType.Unknown || type == ProtocolType.Unspecified)
                _protocolType = ProtocolType.Tcp;
            Selector.SwitchProtocolType(ref _server, type, null);
        }


        public override void Dispose() {

        }

        #endregion

        #region SocketOperation

        public void Send(string clientid, byte[] data) {
            var p = (Package)data.Package();
            beginSend(_clients[0].Socket, p.Serialize(), SocketFlags.None);
        }

        public void SendAsync(string clientid, byte[] data, Action ondatasended) {

        }

        public void BeginAccept() {
            _server.Bind(new IPEndPoint(IPAddress.Any, Port));

            _server.Listen(_maxConnection);
            SocketAsyncEventArgs e = new SocketAsyncEventArgs();
            e.Completed += E_Completed;
            _server.AcceptAsync(e);
        }

        protected void beginReceive(Socket socket) {
            Debug.Write(socket.RemoteEndPoint.ToString());

            SocketAsyncEventArgs e = new SocketAsyncEventArgs();
            e.UserToken = socket;
            e.Completed += OnClientMessageReceive;
            e.SocketFlags = SocketFlags.None;
            e.RemoteEndPoint = socket.RemoteEndPoint;
            if (e.Buffer is null)
            e.SetBuffer(new byte[1024], 0, 1024);

            socket.ReceiveFromAsync(e);
        }

        protected void beginSend(Socket socket, byte[] data, SocketFlags flag) {
            SocketAsyncEventArgs e = new SocketAsyncEventArgs();
            e.UserToken = socket;
            if(e.Buffer is null)
            e.SetBuffer(data, 0, data.Length);
            e.SocketFlags = flag;
            e.Completed += OnMessageSended;
            socket.SendAsync(e);
        }

        private static string getClinetID(HashSet<string> refer) {
            string id = Guid.NewGuid().ToString("");
            id = refer.Contains(id) ? getClinetID(refer) : id;
            return id;
        }

        private void E_Completed(object sender, SocketAsyncEventArgs e) {
            if (e.SocketError == SocketError.Success && e.AcceptSocket is Socket sock && sock != null) {
                Client client = new Client {
                    Socket = sock,
                    ID = getClinetID(_activeClients)
                };
                _onClientAccepted?.Invoke(client);
                _clients.Add(client);
                _activeClients.Add(client.ID);

                beginReceive(sock);

                Timer t = new Timer();
                t.Elapsed += new ElapsedEventHandler((send, e) => T_Elapsed(client, e));
                t.Interval = ActiveTestInterval;
                t.AutoReset = true;

                _clientsTimers.Add(client.ID, t);
                _clientsTimers[client.ID].Start();
            }
            if (_clients.Count < MaxConnection) {
                BeginAccept();
            }
        }

        private void OnMessageSended(object sender, SocketAsyncEventArgs e) {
            e.Dispose();
        }

        private void OnClientMessageReceive(object sender, SocketAsyncEventArgs e) {
            Socket me = e.UserToken as Socket;
            if (e.SocketError == SocketError.Success) {
                if (e.Buffer.UnPackage() is Package p)
                    invokeReceive(p);
            }
            e.Dispose();
            beginReceive(me);
        }

        private void T_Elapsed(Client sender, ElapsedEventArgs e) {
            //if (_activeClients.Contains(sender.ID)) {
            //    _activeClients.Remove(sender.ID);
            //    beginSend(sender.Socket, null, SocketFlags.None);
            //} else {
            //    _clients.Remove(sender);
            //    _clientsTimers[sender.ID].Stop() ;
            //    _clientsTimers[sender.ID].Dispose();
            //    _clientsTimers.Remove(sender.ID);
            //}
        }

        #endregion
    }
}
