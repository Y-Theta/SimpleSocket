///------------------------------------------------------------------------------
/// @ Y_Theta
///------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SocketCore.Core {

    /// <summary>
    /// 用于扩展用到的一些静态方法
    /// </summary>
    public static class MsgExtend {

        #region Methods
        /// <summary>
        /// 从 byte[] 转化为 MsgHead
        /// </summary>
        /// <param name="bytes">需要转化的字节流(要求为8个字节)</param>
        /// <returns></returns>
        public static IPackageMessage ToMsgHead(this byte[] bytes) {
            var mh = new MsgHead {
                PACKID = bytes[0] + (bytes[1] << 8) + (bytes[2] << 16) + (bytes[3] << 24),
                PACKSIZE = (UInt32)(bytes[4] + (bytes[5] << 8) + (bytes[6] << 16) + (bytes[7] << 24)),
            };
            return mh;
        }

        /// <summary>
        /// 将 MsgHead 转化为 byte[]
        /// </summary>
        /// <param name="mh"></param>
        public static byte[] ToByteArray(this IPackageMessage mh) {
            return new byte[] {
                 (byte)((mh.PACKID) & 0xFF),
                  (byte)((mh.PACKID >> 8) & 0xFF),
                   (byte)((mh.PACKID >> 16) & 0xFF),
                    (byte)((mh.PACKID >> 24) & 0xFF),
                     (byte)((mh.PACKSIZE) & 0xFF),
                      (byte)((mh.PACKSIZE >> 8) & 0xFF),
                       (byte)((mh.PACKSIZE >> 16) & 0xFF),
                        (byte)((mh.PACKSIZE >> 24) & 0xFF),
            };
        }

        /// <summary>
        /// 序列化 MsgHead 头部信息
        /// </summary>
        /// <param name="head"></param>
        public static string Serialize(this IPackageMessage head, string format = null) {
            string exp = string.IsNullOrEmpty(format) ? "<  ID :{0} - {1}  >" : format;
            return string.Format(exp, head.PACKID.ToString().PadRight(8), head.PACKSIZE.ToString().PadRight(12));
        }
        #endregion

    }
}
