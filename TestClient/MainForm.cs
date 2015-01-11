using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using Message;
using System.Threading;

namespace TestClient
{
    public partial class MainForm : Form
    {
        UDP.Client client;
        public string name;
        static string LogonKey = "LOGONMESSAGE";

        public MainForm(string userName)
        {
            InitializeComponent();
            client = new UDP.Client(new System.Net.IPEndPoint(IPAddress.Any, 0));
            client.RecvDatagram += new UDP.NetEventArgsHandler(client_RecvDatagram);
            this.name = userName;
        }

        void client_RecvDatagram(object sender, UDP.NetEventArgs e)
        {
            this.richTextBox1.BeginInvoke(new UDP.NetEventArgsHandler(RecvDatagram), new object[] { sender, e });
        }

        void RecvDatagram(object sender, UDP.NetEventArgs e)
        {
            ReceiveMsg msg = new ReceiveMsg(e.Buff);

            if (msg.message == LogonKey)
            {
                SendMsg logMsg = new SendMsg("Server", name);
                logMsg.SendMsgToServer(client);
            }
            else if (msg.toName == name)
            {
                richTextBox1.SelectionColor = Color.Black;
                richTextBox1.SelectionBackColor = Color.Silver;

                richTextBox1.AppendText(string.Format("{0} 说：({1})\n   {2}\n\n", msg.fromName, msg.sendTime.ToString("yy/MM/dd HH:mm:ss"), msg.message)); 

            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            if (this.textBox1.Text == "")
            {
                MessageBox.Show("没有要发送的消息!!!");
            }
            else if (this.ToNameTextBox.Text == "")
            {
                MessageBox.Show("未输入收件方,群发请以“;”分隔"); 
            }
            //test
            string[] sendToNames = this.ToNameTextBox.Text.Replace("；", ";").Split(';');
            if (this.textBox1.Text != "" && this.ToNameTextBox.Text != "")
            {
                foreach (string sendToName in sendToNames)
                {
                    Message.SendMsg msg = new Message.SendMsg(sendToName, this.name, DateTime.Now, this.textBox1.Text, this.textBox1.Text);
                    msg.SendMsgToServer(client);
                    textBox1.Text = "";

                    richTextBox1.SelectionColor = Color.White;
                    richTextBox1.SelectionBackColor = Color.Green;
                    richTextBox1.AppendText(string.Format("{0} 说：({1})\n   {2}\n\n", msg.fromName, msg.sendTime.ToString("yy/MM/dd HH:mm:ss"), msg.messagel));

                }
            }
        }

        private void MainForm_Activated(object sender, EventArgs e)
        {
            SendMsg logMsg = new SendMsg("Server", name);
            logMsg.SendMsgToServer(client);
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Dispose(true);
            this.Close();
            Application.Exit();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}