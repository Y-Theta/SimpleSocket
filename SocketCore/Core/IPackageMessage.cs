///------------------------------------------------------------------------------
/// @ Y_Theta
///------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SocketCore.Core {
    public interface IPackageMessage {
        #region Properties
        Int32 PACKID { get; set; }

        UInt32 PACKSIZE { get; set; }
        #endregion
    }
}
