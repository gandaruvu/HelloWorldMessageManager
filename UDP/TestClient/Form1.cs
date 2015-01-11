using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;

namespace TestClient
{
    public partial class Form1 : Form
    {

        UDP.Client client;
        EndPoint serverEP;

        public Form1()
        {
            InitializeComponent();
            client = new UDP.Client(new System.Net.IPEndPoint(IPAddress.Any, 0));
            client.RecvDatagram += new UDP.NetEventArgsHandler(client_RecvDatagram);
        }

        void client_RecvDatagram(object sender, UDP.NetEventArgs e)
        {
            this.listBox1.BeginInvoke(new UDP.NetEventArgsHandler(RecvDatagram), new object[] { sender, e });
        }

        void RecvDatagram(object sender, UDP.NetEventArgs e)
        {
            listBox1.Items.Add(string.Format("接收到{0}的消息,内容:{1}", e.RemoteEP.ToString(), System.Text.Encoding.Unicode.GetString(e.Buff)));
        }

        
        private void button1_Click(object sender, EventArgs e)
        {
            if (this.textBox1.Text == "")
            {
                MessageBox.Show("没有要发送的消息!!!");
            }
            if (this.textBox2.Text == "")
            {
                MessageBox.Show("没有指定服务器IP!!!");
            }
            if (this.textBox3.Text == "")
            {
                MessageBox.Show("没有指定服务器端口!!!");
            }
            if (this.serverEP == null)
            {
                serverEP = new System.Net.IPEndPoint(IPAddress.Parse(this.textBox2.Text), int.Parse(this.textBox3.Text));
            }
            client.SendTo(System.Text.Encoding.Unicode.GetBytes(this.textBox1.Text), serverEP);
        }
    }
}