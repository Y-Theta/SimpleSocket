using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SocketCore.DataModel;

namespace SocketCore.FileOperator {

    /// <summary>
    /// 打包队列消息参数
    /// </summary>
    public class FileQueueArgs : EventArgs {

        /// <summary>
        /// 队列ID
        /// </summary>
        public int QueueID { get; set; }

        /// <summary>
        /// 队列信息初始值
        /// </summary>
        public QueueInfo Origin { get; set; }

        /// <summary>
        /// 队列信息当前值
        /// </summary>
        public QueueInfo Now { get; set; }

        public FileQueueArgs(int queueID, QueueInfo origin, QueueInfo now) {
            QueueID = queueID;
            Origin = origin;
            Now = now;
        }
    }
}
