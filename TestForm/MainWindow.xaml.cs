using SocketCore.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json;

namespace TestForm {
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
        }

        private unsafe void MainWindow_Loaded(object sender, RoutedEventArgs e) {
            byte[] o = Encoding.Default.GetBytes("12345678910");
            Package p = (Package)o.Package();

            Console.WriteLine(p.Info());
            byte[] s = p.Serialize();
            Console.WriteLine(JsonConvert.SerializeObject(p));
            Package b = (Package)s.UnPackage();
            Console.WriteLine(JsonConvert.SerializeObject(b));
            Console.WriteLine(b.Info());
            //RecieiveAsync(result);
        }

        private void result(IAsyncResult ar) {
            int result = ((Func<int>)ar.AsyncState).EndInvoke(ar);
        }

        private int test() {
            return 1;
        }

        public void RecieiveAsync(AsyncCallback callback) {
            Func<int> call = test;
            call.BeginInvoke(callback, call);
        }
    }
}
