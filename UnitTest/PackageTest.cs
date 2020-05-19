using System.Diagnostics;
using System.Text;
using NUnit.Framework;
using SocketCore;
using SocketCore.Core;
using Newtonsoft.Json;
using System.Runtime.InteropServices;

namespace UnitTest {


    public class PackageTest {



        [SetUp]
        public void Setup() {
        }



        [Test]
        public void PackTest() {
            string info = "Hello World ,I am a simple package !";
            byte[] infodata = Encoding.ASCII.GetBytes(info);
            Package infopack = (Package)infodata.Package();

            Assert.AreSame(infodata, infopack.DATA);

            byte[] transfer = infopack.Serialize();

            Package inforec = (Package)transfer.UnPackage();

            Assert.AreEqual(inforec.DATA, infopack.DATA ,"Not Equal");
            Assert.AreNotSame(inforec.DATA, infopack.DATA, $" ORI: {JsonConvert.SerializeObject(infopack.DATA)}   REC: {JsonConvert.SerializeObject(inforec.DATA)} ");
        }
    }
}