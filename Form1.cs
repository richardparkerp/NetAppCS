using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using System.Net.Sockets;
using System.Net;

namespace ExamCS
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            cbTables.Items.Add("first table");
            cbTables.Items.Add("second table");
            cbTables.Items.Add("third table");
            cbTables.Items.Add("fourth table");
            cbTables.Items.Add("fifth table");

            Thread.Sleep(2000);
            cbTables.SelectedIndex = 1;

            ThreadPool.SetMinThreads(5, 5);
            ThreadPool.SetMaxThreads(100, 100);


            tcpClient = new TcpClient();
            try
            {
                tcpClient.Connect(IPAddress.Parse(ip), srvPort);

                if (tcpClient.Connected)
                {
                    NetworkStream ns = tcpClient.GetStream();
                    size = ns.Read(buffer, 0, buffer.Length);
                    string msg = Encoding.UTF8.GetString(buffer, 0, size);
                    if (msg == "kitchen is ready")
                    {
                        ThreadPool.QueueUserWorkItem(ClientProc, this);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public AutoResetEvent orderWait = new AutoResetEvent(false);


        public void ClientProc(object obj)
        {
            Form1 form = obj as Form1;

            try
            {
                form.tcpClient.Client.BeginReceive(form.buffer, 0, form.buffer.Length, SocketFlags.None, CallbackReceive, form);

                while (form.tcpClient.Connected)
                {
                    orderWait.WaitOne();
                    string message = msg;
                    form.tcpClient.Client.Send(Encoding.UTF8.GetBytes(message));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                form.tcpClient.Close();
                form.tcpClient = null;
            }


        }

        private void CallbackReceive(IAsyncResult ar)
        {
            if (ar.IsCompleted)
            {
                Form1 form = ar.AsyncState as Form1;
                try
                {
                    int size = form.tcpClient.Client.EndReceive(ar);
                    string temp = Encoding.UTF8.GetString(form.ResBuff, 0, size);
                    form.Invoke((Action)delegate ()
                    {
                        form.txtMessageFrom.Text += temp + "order is ready! \r\n";
                        form.Invalidate(true);
                        form.Update();
                    });

                    form.tcpClient.Client.BeginReceive(form.ResBuff,0, form.ResBuff.Length, SocketFlags.None, CallbackReceive, form);

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }


        public IPAddress srvIP;
        public int srvPort = 12345;

        public TcpClient tcpClient = null;
        public string ip = "127.0.0.1";


        public Socket socket = null;
        public List<Dish> Order = new List<Dish>();
        public int size;
        public byte[] buffer = new byte[4 * 1024];
        public byte[] ResBuff = new byte[4 * 1024];
        public string msg;
        public Mutex mutex = new Mutex();




        private void button1_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            Dish dish = new Dish(btn.Text);
            Order.Add(dish);
            txtOrder.Text += dish.Name + ',' + "\r\n";
        }

        private void button19_Click(object sender, EventArgs e)  //кнопка очистить заказ 
        {
            Order = new List<Dish>();
            txtOrder.Text = "";
        }
        public Random rnd = new Random();
        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (Order.Count > 0)
            {
                txtOrder.Text.Trim(',');
                int idx = cbTables.SelectedIndex + 1;
                msg = rnd.Next(20).ToString() + ';' + idx.ToString() + ';' + txtOrder.Text + ';' + $"{txtMessageTo.Text}";

                orderWait.Set();

                Order.Clear();
                txtOrder.Text = "";
            }

        }




        public class Dish
        {
            public string Name { get; set; }
            public string Compound { get; set; }  //состав

            public int Price { get; set; }

            public int Time { get; set; }

            public Dish(string name) { Name = name; }
            public Dish(string name, int price) { Name = name; Price = price; }
        }
    }
}
