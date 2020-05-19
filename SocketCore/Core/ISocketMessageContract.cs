using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SocketCore.Core {
    public interface ISocketMessageContract :IDisposable {
        
        #region Properties
        /// <summary>
        /// IP
        /// </summary>
        IPAddress Address { get; }

        /// <summary>
        /// PORT
        /// </summary>
        int Port { get; }

        /// <summary>
        /// 连接标识符
        /// </summary>
        bool Actived { get; }

        /// <summary>
        /// 
        /// </summary>
        event PackageResolveHandle OnMessageReceive;
        #endregion

        #region Methods

        #region SYS

        /// <summary>
        /// 初始化客户端
        /// </summary>
        void Init();

        #endregion

        #region MSG

        /// <summary>
        /// 发送信息
        /// </summary>
        /// <param name="data">待发送的数据</param>
        void Send(Package data);

        void SendAsync(Package data, Action ondatasended);

        #endregion

        #endregion

    }
}
