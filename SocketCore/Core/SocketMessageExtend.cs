///------------------------------------------------------------------------------
/// @ Y_Theta
///------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;


namespace SocketCore.Core {

    /// <summary>
    /// 用于扩展用到的一些静态方法
    /// </summary>
    public static class SocketMessageExtend {

        #region Methods
        /// <summary>
        /// 将 byte[] 包装为 Package
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static Package? Package(this byte[] bytes, int? pid = null, long? poffset = null, int? cid = null) {
            if (bytes == null) return null;
            Package p = new Package();
            p.PACKID = pid == null ? int.MinValue : (int)pid;
            p.CID = cid == null ? int.MinValue : (int)cid;
            p.PACKOFFSET = poffset == null ? long.MinValue : (long)poffset;
            p.PACKSIZE = bytes.Length;
            p.DATA = bytes;
            return p;
        }

        /// <summary>
        /// 将包转化为字节流以传输
        /// </summary>
        /// <param name="package"></param>
        /// <returns></returns>
        public static byte[] Serialize(this Package package) {
            //byte[] tosend = new byte[32 + package.PACKSIZE];
            List<byte> result = new List<byte>(28 + (int)package.PACKSIZE);
            result.AddRange(BitConverter.GetBytes(package.CID));
            result.AddRange(BitConverter.GetBytes(package.PACKID));
            result.AddRange(BitConverter.GetBytes(package.PACKOFFSET));
            result.AddRange(BitConverter.GetBytes(package.PACKSIZE));
            result.AddRange(BitConverter.GetBytes(package.Reserved));
            result.AddRange(package.DATA);
            return result.ToArray();
        }

        /// <summary>
        /// 解析字节流生成包
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static Package? UnPackage(this byte[] bytes) {
            Package p = new Package();
            p.CID = BitConverter.ToInt32(bytes, 0);
            p.PACKID = BitConverter.ToInt32(bytes, 4);
            p.PACKOFFSET = BitConverter.ToInt64(bytes, 8);
            p.PACKSIZE = BitConverter.ToInt32(bytes, 16);
            p.Reserved = BitConverter.ToInt64(bytes, 20);
            p.DATA = new byte[p.PACKSIZE];
            Array.Copy(bytes, 28, p.DATA, 0, p.PACKSIZE);
            return p;
        }

        /// <summary>
        /// 将 MsgHead 转化为 byte[]
        /// </summary>
        /// <param name="mh"></param>
        //public static byte[] ToByteArray(this PackageHead mh) {
        //    return new byte[] {
        //         (byte)((mh.PACKID) & 0xFF),
        //          (byte)((mh.PACKID >> 8) & 0xFF),
        //           (byte)((mh.PACKID >> 16) & 0xFF),
        //            (byte)((mh.PACKID >> 24) & 0xFF),
        //             (byte)((mh.PACKSIZE) & 0xFF),
        //              (byte)((mh.PACKSIZE >> 8) & 0xFF),
        //               (byte)((mh.PACKSIZE >> 16) & 0xFF),
        //                (byte)((mh.PACKSIZE >> 24) & 0xFF),
        //    };
        //}

        /// <summary>
        /// 序列化 MsgHead 头部信息
        /// </summary>
        /// <param name="head"></param>
        public static string Info(this Package head, string format = null) {
            string exp = string.IsNullOrEmpty(format) ? "< {0} - {1} : {2}  >" : format;
            return string.Format(exp,
                head.CID.ToString().PadRight(20),
                head.PACKID.ToString().PadRight(20),
                head.PACKSIZE.ToString().PadRight(20));
        }
        #endregion

    }
}
