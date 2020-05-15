using SocketCore.Core;
using System;
using System.Collections.Generic;
using System.Linq;
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

namespace TestForm {
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e) {
            MsgHead h = new MsgHead();
            h.PACKID = 12;
            h.PACKSIZE = 4096;
            Console.WriteLine(h.Serialize());
            Console.WriteLine(h.ToByteArray());
            RecieiveAsync(result);
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
