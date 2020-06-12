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

    /// <summary>
    /// socket 基类
    /// </summary>
    public abstract class SocketBase : ISocketMessageContract {
        #region Properties
        /// <summary>
        /// IP 地址
        /// </summary>
        public IPAddress Address { get; set; }

        /// <summary>
        /// 端口
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// 用于指示Socket状态
        /// </summary>
        public bool Actived { get; set; }

        /// <summary>
        /// Socket类型
        /// </summary>
        public ProtocolType ProtocolType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ISocketTypeSelector Selector { get; set; }

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
        /// <summary>
        /// 触发接收回调
        /// </summary>
        /// <param name="p"></param>
        protected void invokeReceive(Package p) {
            _onmessageReceived?.Invoke(p);
        }

        /// <summary>
        /// 切换Socket协议模式
        /// </summary>
        protected abstract void switchProtocolType(ProtocolType type);

        public abstract void Init();

        public abstract void Dispose();

        #endregion

        #endregion

        #region Constructors
        #endregion


    }
}
