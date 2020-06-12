using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SocketCore.Core {

    /// <summary>
    /// socket 通信约定
    /// </summary>
    public interface ISocketMessageContract : IDisposable {

        #region Properties
        /// <summary>
        /// 获取或设置Socket类型
        /// <para>
        /// 可用类型可从以下项目选取
        /// </para>
        /// <para></para>
        /// <see cref="ProtocolType.Tcp"/><para></para>
        /// <see cref="ProtocolType.Udp"/>
        /// </summary>
        ProtocolType ProtocolType { get; }

        /// <summary>
        /// 
        /// </summary>
        ISocketTypeSelector Selector { get; }

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
        /// 初始化Socket
        /// </summary>
        void Init();

        #endregion

        #region MSG


        #endregion

        #endregion

    }

    /// <summary>
    /// 定义如何切换Socket类型
    /// </summary>
    public interface ISocketTypeSelector {

        /// <summary>
        /// 通过此方法切换socket的类型
        /// </summary>
        /// <param name="role"></param>
        /// <param name="protocoltype"></param>
        /// <param name="action"></param>
        void SwitchProtocolType(ref Socket socket, ProtocolType protocoltype, Action<ProtocolType> action);
    }
}
