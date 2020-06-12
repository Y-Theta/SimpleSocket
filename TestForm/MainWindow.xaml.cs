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
using System.Threading;
using System.Drawing.Printing;
using System.Drawing;
using Rectangle = System.Drawing.Rectangle;
using Brushes = System.Drawing.Brushes;
using System.IO.MemoryMappedFiles;
using System.Diagnostics;
using SocketCore.Extension;
using System.Net;
using SocketCore.DataModel;
using Microsoft.Win32;

namespace TestForm {
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window, ICommand {

        int[] data;
        static readonly object _lock = new object();

        private static string[] testlabel = new string[] {
            "Test - 1",
            "Test - 2",
            "Test - 10",
            "Test - 111",
        };
        public static string[] TestLabel { get => testlabel; set => testlabel = value; }

        public Dictionary<string, TestStatic> Test;

        int i = 0;

        public event EventHandler CanExecuteChanged;
        const string file = "E:\\MapBigData.dat";

        public MainWindow() {
            InitializeComponent();

            this.Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e) {
            //byte[] o = Encoding.Default.GetBytes("12345678910");
            //Package p = (Package)o.Package();
            //data = new int[20];
            //Console.WriteLine(Thread.CurrentThread.ManagedThreadId);
            Console.WriteLine(Marshal.SizeOf(typeof(FilePackageInfo)));
            //RecieiveAsync(result);
            //for (int i = 10; i > 0; i--) {
            //    var s = i.ToString();
            //    Task.Run(() => {
            //        TestMTA(s);
            //    });
            //}

        }

        void TestMTA(string str) {
            lock (_lock) {
                Console.WriteLine("---------------------------------------");
                Console.WriteLine(str);
                Console.WriteLine(Thread.CurrentThread.ManagedThreadId);
                Console.WriteLine(data.Length);
                Console.WriteLine(JsonConvert.SerializeObject(data));
                Console.WriteLine("---------------------------------------");
                data[0] = int.Parse(str);
            }

        }

        private void result(IAsyncResult ar) {
            int result = ((Func<int>)ar.AsyncState).EndInvoke(ar);
            Console.WriteLine(result);
            Console.WriteLine(Thread.CurrentThread.ManagedThreadId);
        }

        private int test() {
            return 1;
        }

        public void RecieiveAsync(AsyncCallback callback) {
            Func<int> call = test;
            call.BeginInvoke(callback, call);
        }

        private void Pd_PrintPage(object sender, PrintPageEventArgs e) {


        }

        #region Icommand
        public bool CanExecute(object parameter) {
            return true;
        }

        public void Execute(object parameter) {
            Console.WriteLine(parameter);
            switch (parameter) {
                case "Test":
                    //Console.WriteLine("Test");
                    //FileStream fs = new FileStream(file, FileMode.CreateNew);
                    //fs.Seek(32L * 1024 * 1024 - 1, SeekOrigin.Begin);
                    //fs.WriteByte(0b11110000);
                    //fs.Close()
                    //ser = new SocketServerBase();
                    //ser.Address = new System.Net.IPAddress(new byte[] { 127, 0, 0, 1 });
                    //ser.ActiveTestInterval = 10000;
                    //ser.Port = 24680;
                    //ser.Init();
                    //ser.OnClientAccepted += Ser_OnClientAccepted;
                    //ser.OnMessageReceive += Ser_OnMessageReceive;
                    //ser.BeginAccept();
                    //using (MemoryMappedFile mf = MemoryMappedFile.CreateFromFile(file, FileMode.Open, "testmap", 0)) {
                    //    using (MemoryMappedViewAccessor ma = mf.CreateViewAccessor(1024L * (32L * 1024 - 1), 1024L)) {
                    //        byte b = ma.ReadByte(1023);
                    //        Console.WriteLine(b);
                    //    }
                    //};

                    //FileDialog dia = new OpenFileDialog();
                    //dia.FileOk += Dia_FileOk;
                    //dia.Filter = "*.txt,*.dat|*.dat;*.txt|所有文件(*.*)|*.*";
                    //dia.ShowDialog();
                    //Test = new Dictionary<string, TestStatic>();
                    //for (int i = 0; i < 3; i++) {
                    //    var temp = i.ToString();

                    //    Task.Run(() => {
                    //        lock (Test) {

                    //            Console.WriteLine(temp);
                    //            Console.WriteLine(TestStatic.Data);
                    //            Console.WriteLine(TestStatic.TData);
                    //            Console.WriteLine(TestStatic.List == null ? "null" : TestStatic.List[0]);
                    //            Console.WriteLine(TestStatic.TList == null ? "null" : TestStatic.TList[0]);
                    //            TestStatic test = new TestStatic { Name = "Test - " + temp };
                    //            TestStatic.Data = int.Parse(temp);
                    //            TestStatic.TData = int.Parse(temp);
                    //            TestStatic.List = new List<string> { temp };
                    //            TestStatic.TList = new List<string> { temp };
                    //            Console.WriteLine();
                    //            Console.WriteLine();

                    //            Test.Add(test.Name, test);
                    //        }
                    //    });
                    //}



                    break;
                case "MuiltThreadAccess":


                    //cli = new SocketClientBase();
                    //cli.Address = new System.Net.IPAddress(new byte[] { 127, 0, 0, 1 });
                    //cli.Port = 24680;
                    //cli.Init();
                    //cli.OnMessageReceive += Cli_OnMessageReceive;
                    //cli.BeginConnent(2000);
                    break;

                case "Send":
                    //cli?.Send(Encoding.ASCII.GetBytes(toSend.Text));
                    //ser?.Send(" ", Encoding.ASCII.GetBytes(toSend.Text));
                    break;
            }
        }

        private void Dia_FileOk(object sender, System.ComponentModel.CancelEventArgs e) {
            FileDialog fd = ((FileDialog)sender);
            FileInfo fi = new FileInfo(fd.FileName);
            Console.WriteLine(fi.Length / 1024 / 1024);
            Console.WriteLine(fi.Name);
            Console.WriteLine(fi.DirectoryName);
            Console.WriteLine(fi.Attributes);
        }

        //private void Cli_OnMessageReceive(Package package) {
        //    Debug.WriteLine(package.DATA.Length);
        //    App.Current.Dispatcher.Invoke(() => {
        //        Result.Text += "\n" + Encoding.ASCII.GetString(package.DATA);
        //    });
        //}

        //private void Ser_OnClientAccepted(Client client, bool accept) {
        //    Debug.WriteLine(client.Socket.RemoteEndPoint);
        //    App.Current.Dispatcher.Invoke(() => {
        //        Result.Text += client.Socket.RemoteEndPoint.ToString();
        //        Result.Text += client.ID.ToString();
        //    });
        //}

        //private void Ser_OnMessageReceive(Package package) {
        //    Debug.WriteLine(package.DATA.Length);
        //    App.Current.Dispatcher.Invoke(() => {
        //        Result.Text += "\n" + Encoding.ASCII.GetString(package.DATA);
        //    });
        //}


        #endregion
    }


    public class TestStatic {

        public string Name;


        public static int Data;

        [ThreadStatic]
        public static int TData;

        public static List<string> List;

        [ThreadStatic]
        public static List<string> TList;



    }
}
