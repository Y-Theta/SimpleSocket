using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketCore.FileOperator {

    /// <summary>
    /// 压缩接口
    /// </summary>
    public interface ICompress {

        public byte[] Compress(byte[] data);
        public byte[] DeCompress(byte[] data);
    }
}
