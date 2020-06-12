///------------------------------------------------------------------------------
/// @ Y_Theta
///------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SocketCore.DataModel;

namespace SocketCore.Extension {
    public static class UserPackageExtension {
        #region Properties
        #endregion

        #region Methods
        /// <summary>
        /// 将 byte[] 包装为 Package
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static UserPackage Package(this byte[] bytes, int? pid = null, long? poffset = null, int? cid = null) {
            if (bytes == null) return null;
            UserPackage p = new UserPackage();
            p.PACKID = pid == null ? int.MinValue : (int)pid;
            p.CID = cid == null ? int.MinValue : (int)cid;
            p.PACKOFFSET = poffset == null ? long.MinValue : (long)poffset;
            p.PACKSIZE = bytes.Length;
            p.TimeStamp = DateTime.Now.ToUnixTime();
            p.DATA = bytes;
            return p;
        }

        /// <summary>
        /// 将包转化为字节流以传输
        /// </summary>
        /// <param name="package"></param>
        /// <returns></returns>
        public static byte[] Serialize(this UserPackage package) {
            //byte[] tosend = new byte[32 + package.PACKSIZE];
            List<byte> result = new List<byte>(28 + (int)package.PACKSIZE);
            result.AddRange(BitConverter.GetBytes(package.CID));
            result.AddRange(BitConverter.GetBytes(package.PACKID));
            result.AddRange(BitConverter.GetBytes(package.PACKOFFSET));
            result.AddRange(BitConverter.GetBytes(package.PACKSIZE));
            result.AddRange(BitConverter.GetBytes(package.TimeStamp));
            result.AddRange(BitConverter.GetBytes(package.Reserved));
            result.AddRange(package.DATA);
            return result.ToArray();
        }

        /// <summary>
        /// 解析字节流生成包
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static UserPackage UnPackage(this byte[] bytes) {
            UserPackage p = new UserPackage();
            p.CID = BitConverter.ToInt32(bytes, 0);
            p.PACKID = BitConverter.ToInt32(bytes, 4);
            p.PACKOFFSET = BitConverter.ToInt64(bytes, 8);
            p.PACKSIZE = BitConverter.ToInt32(bytes, 16);
            p.TimeStamp = BitConverter.ToDouble(bytes, 20);
            p.Reserved = BitConverter.ToInt32(bytes, 28);
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
        public static string Info(this UserPackage head, string format = null) {
            string exp = string.IsNullOrEmpty(format) ? "< {0} - {1} : {2}   {3}>" : format;
            return string.Format(exp,
                head.CID.ToString().PadRight(20),
                head.PACKID.ToString().PadRight(20),
                head.PACKSIZE.ToString().PadRight(20),
                head.TimeStamp.ToDateTime().ToLongTimeString());
        }

        /// <summary>
        /// 将Unix格式时间转化为Datetime
        /// </summary>
        /// <param name="utctime"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this double utctime) {
            TimeSpan span = TimeSpan.FromSeconds(utctime);
            //TimeSpan span = (DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc));
            DateTime time = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            return time.Add(span);
        }


        /// <summary>
        /// 将 Datetime转化为Unix时间戳
        /// </summary>
        /// <param name="utctime"></param>
        /// <returns></returns>
        public static double ToUnixTime(this DateTime utctime) {
            TimeSpan span = utctime.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            return (int)span.TotalSeconds;
        }
        #endregion


        #region Constructors
        #endregion
    }
}
