using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Kitchen
{
    public partial class KitcheForm : Form
    {
        public KitcheForm()
        {
            InitializeComponent();

            lstW_Orders.Columns.Add("Order Number",80);
            lstW_Orders.Columns.Add("Table Number", 80);
            lstW_Orders.Columns.Add("Order Time", 80);
            lstW_Orders.Columns.Add("Order list",450);
            lstW_Orders.Columns.Add("Message", 150);

            ThreadPool.SetMinThreads(10, 10);
            ThreadPool.SetMaxThreads(100, 100);
            ThreadPool.QueueUserWorkItem(ServerProc, this);
        }

        public byte[] buffer = new byte[4 * 1024];
        public int size;

        public TcpListener tcpServer = null;
        public TcpClient client = null;
        public string srvIP = "0.0.0.0";
        public int srvPort = 12345;

        public AutoResetEvent orderWait = new AutoResetEvent(false);

        public string readyOrder;


        public void ServerProc(object obj)
        {
            KitcheForm form = obj as KitcheForm;

            try
            {
                tcpServer = new TcpListener(IPAddress.Parse(srvIP), srvPort);
                tcpServer.Start(100);


                string msg = "kitchen is ready";
                while (true)
                {

                    client = tcpServer.AcceptTcpClient();

                    //client = form.srvSocket.Accept();
                    //client.Send(Encoding.UTF8.GetBytes(msg));


                    byte[] temp = Encoding.UTF8.GetBytes(msg);
                    client.GetStream().Write(temp, 0, temp.Length);
                    ThreadPool.QueueUserWorkItem(ClientProc, form);

                    //получаем строку в следующем виде "номер заказа + ; + номер столика + ; + заказы через запятую + ; + сообщения для повара или же просто 0"

                    /* string[] temp = msg.Split(';');
                     string orderNumber = temp[0];
                     string tableNumber = temp[1];
                     string[] orderArr = temp[2].Split(',');
                     string messageToKitchen = temp[3];
                     DateTime dt = new DateTime();
                     ListViewItem newitem = new ListViewItem(orderNumber);
                     newitem.SubItems.Add(tableNumber);
                     newitem.SubItems.Add(DateTime.Now.ToShortTimeString());
                     lstW_Orders.Items.Add(newitem);*/

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }//ThreadProc

        public void ClientProc(object obj)
        {
            KitcheForm form = obj as KitcheForm;

            try
            {
                form.client.Client.BeginReceive(form.buffer, 0, form.buffer.Length, SocketFlags.None, CallbackReceive, form);

                while (form.client.Connected)
                {

                    orderWait.WaitOne();
                    string msg = readyOrder;
                    form.client.Client.Send(Encoding.UTF8.GetBytes(msg));

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                form.client.Close();
                form.client = null;

            }
        }

        private void CallbackReceive(IAsyncResult ar)
        {
            if (ar.IsCompleted)
            {
                KitcheForm form = ar.AsyncState as KitcheForm;
                try
                {
                    int size = form.client.Client.EndReceive(ar);
                    string msg = Encoding.UTF8.GetString(form.buffer, 0, size);

                    //получаем строку в следующем виде "номер заказа + ; + номер столика + ; + заказы через запятую + ; + сообщения для повара или же просто 0"

                    string[] temp = msg.Split(';');
                    string orderNumber = temp[0];
                    string tableNumber = temp[1];
                    string orderLst = toString(temp[2].Split(','));
                    string messageToKitchen = temp[3];
                    form.Invoke((Action)delegate ()
                    {
                        ListViewItem newitem = new ListViewItem(orderNumber);
                        newitem.SubItems.Add(tableNumber);
                        newitem.SubItems.Add(DateTime.Now.ToShortTimeString());
                        newitem.SubItems.Add(orderLst);
                        newitem.SubItems.Add(messageToKitchen);
                        lstW_Orders.Items.Add(newitem);

                        form.Invalidate();
                        form.Update();
                    });


                    form.client.Client.BeginReceive(
                           form.buffer, 0, form.buffer.Length,
                           SocketFlags.None,
                           CallbackReceive, form);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }


            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (lstW_Orders.SelectedItems.Count > 0)
            {
                lstW_Orders.Items.Remove(lstW_Orders.SelectedItems[0]);
                
                string asd = lstW_Orders.SelectedItems.ToString();
                readyOrder = asd.ToString();
                orderWait.Set();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
           readyOrder = lstW_Orders.Items.ToString();
            lstW_Orders.Items.Clear();
            orderWait.Set();
        }

        public string toString(string [] arr)
        {
            string asd = "";
            for (int i = 0; i < arr.Length; i++)
            {
                asd += arr[i].ToString() + ';';
            }
            asd.Trim(',');

            return asd;
        }
      
    }
}
