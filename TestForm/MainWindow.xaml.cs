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
using SocketCore.FileOperator;
using System.Collections;

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

        FilePackager fp;
        int i = 0;

        public event EventHandler CanExecuteChanged;
        const string file = "E:\\MapBigData.dat";

        public MainWindow() {
            InitializeComponent();

            this.Loaded += MainWindow_Loaded;
        }

        private unsafe void MainWindow_Loaded(object sender, RoutedEventArgs e) {
            //byte[] o = Encoding.Default.GetBytes("12345678910");
            //Package p = (Package)o.Package();
            //data = new int[20];
            //Console.WriteLine(Thread.CurrentThread.ManagedThreadId);

            Console.WriteLine(UInt32.MaxValue);
            Console.WriteLine(Int32.MaxValue);
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


                    FileDialog fd = new OpenFileDialog();
                    fd.AddExtension = true;
                    fd.CheckFileExists = true;
                    fd.FileOk += Dia_FileOk;
                    fd.Filter = "dat files|*.dat|All files (*.*)|*.*";
                    fd.ShowDialog();
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


            OpenFileDialog fd = ((OpenFileDialog)sender);
            MemoryMappedFile file = MemoryMappedFile.CreateFromFile(fd.FileName, FileMode.Open, fd.SafeFileName + "cached",0, MemoryMappedFileAccess.ReadWrite);

            MemoryMappedViewAccessor acc = file.CreateViewAccessor(0, (long)1 << 31 - 1);


            //List<Task> ts = new List<Task>();
            //for (int i = 0; i < 5; i++) {
            //    int index = i;
            //    Task t = new Task(() => {
            //        var accessor = file.CreateViewAccessor(index * 128, 128, MemoryMappedFileAccess.Write);
            //        char[] buffer = "Hellow World".ToArray();
            //        accessor.WriteArray(0, buffer, 0, buffer.Length);
            //        accessor.Flush();
            //        accessor.Dispose();
            //    });
            //    ts.Add(t);
            //}
            //ts.ForEach(t => t.Start());
            //Task.WaitAll(ts.ToArray());
            //file.Dispose();

            //fp = new FilePackager();
            //FileQueue queue = null;
            //FilePackageInfo info = new FilePackageInfo();
            //Thread.Sleep(500);
            //fp.Package(fd.FileName, 4, ref queue, ref info, OnReady, OnProgress);
            //Thread.Sleep(500);

            //Console.WriteLine(JsonConvert.SerializeObject(info));
        }

        private void OnProgress(object sender, FileQueueArgs e) {
            FileQueue q = sender as FileQueue;

            lock (q.PackageQueues) {
                Console.WriteLine("-------------------------------------");
                Console.WriteLine("Progressing : " + e.QueueID);
                Console.WriteLine(JsonConvert.SerializeObject(e.Origin));
                Console.WriteLine(JsonConvert.SerializeObject(e.Now));
                Console.WriteLine("-------------------------------------");
            }
        }

        private void OnReady(object sender, FileQueueArgs e) {
            FileQueue q = sender as FileQueue;

            lock (q.PackageQueues) {
                Console.WriteLine("-------------------------------------");
                Console.WriteLine("Ready : " + e.QueueID);
                Console.WriteLine(JsonConvert.SerializeObject(e.Origin));
                Console.WriteLine(JsonConvert.SerializeObject(e.Now));
                Console.WriteLine("-------------------------------------");

                //if (e.QueueID == 1) {
                //    Task.Run(() => {
                //        for (int i = 0; i < 512; i++) {
                //            UserPackage p = null;
                //            while (!q.Get(1, out p)) {
                //                Thread.Sleep(5);
                //            }
                //            Console.WriteLine(p?.Info());
                //        }
                //    });
                //}
            }
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
}
