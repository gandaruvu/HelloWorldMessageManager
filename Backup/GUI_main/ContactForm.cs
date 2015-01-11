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
using UDP;
using Message;

namespace GUI_main
{
    public partial class ContactForm : Form
    {
        public string myName = string.Empty;
        UDP.Client client;
        public ContactForm(string userName)
        {
            this.myName = userName;
            client = new UDP.Client(new System.Net.IPEndPoint(IPAddress.Any, 0));
            client.RecvDatagram += new UDP.NetEventArgsHandler(client_RecvDatagram);
            InitializeComponent();
            LoadContact(cttDummy);
        }

        #region WinformAct
        Point mousePos;
        bool leftDown;
        bool butten_over;
        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && !butten_over)
            {
                mousePos = new Point(-e.X, -e.Y); //得到变量的值
                leftDown = true;                  //点击左键按下时标注为true;
            }
        }
        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (leftDown)
            {
                Point mouseSet = Control.MousePosition;
                mouseSet.Offset(mousePos.X, mousePos.Y);  //设置移动后的位置
                Location = mouseSet;
            }
        }
        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            if (leftDown)
            {
                leftDown = false;
            }
        }
        //关闭按钮 //     
        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void button1_MouseHover(object sender, EventArgs e)
        {
            butten_over = true;
            this.button1.BackgroundImage = global::GUI_main.Properties.Resources.close_over;
        }
        private void button1_MouseLeave(object sender, EventArgs e)
        {
            butten_over = false;
            this.button1.BackgroundImage = global::GUI_main.Properties.Resources.close;
        }
        private void button1_MouseDown(object sender, MouseEventArgs e)
        {
            this.button1.BackgroundImage = global::GUI_main.Properties.Resources.close_down;
        }
        //最小化按钮//
        private void button2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        private void button2_MouseDown(object sender, MouseEventArgs e)
        {
            this.button2.BackgroundImage = global::GUI_main.Properties.Resources.mini_down;
        }
        private void button2_MouseHover(object sender, EventArgs e)
        {
            butten_over = true;
            this.button2.BackgroundImage = global::GUI_main.Properties.Resources.mini_over;
        }
        private void button2_MouseLeave(object sender, EventArgs e)
        {
            butten_over = false;
            this.button2.BackgroundImage = global::GUI_main.Properties.Resources.mini;
        }
        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

            foreach (ListViewItem item in this.listView1.SelectedItems)
            {
                item.BackColor = Color.FromArgb(250, 250, 150);
                item.ImageIndex = int.Parse(item.Name) > 0 ? 3 : 2;
                if (selected != null)
                {
                    selected.BackColor = Color.FromArgb(232, 232, 228);
                    selected.ImageIndex = int.Parse(selected.Name) > 0 ? 1 : 0;
                }
                selected = this.listView1.FocusedItem;
                this.listView1.FocusedItem.Selected = false;
            }

        }
        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            MessageBox.Show(selected.Name);
        }
        #endregion

        #region ReadContact
        private ListViewItem selected;
        private struct Contact
        {
            public string num;
            public DateTime lastTime;
            public int newMsg;
        }
        private struct cttLv
        {
            public string num;
            public int newMsg;
            public cttLv(string nnum, int nnewMsg)
            {
                num = nnum;
                newMsg = nnewMsg;
            }
        }
        Contact[] cttDummy ={  new Contact{num="10010",lastTime=new DateTime(2014,9,11,9,20,31),newMsg=0},
                                new Contact{num="10086",lastTime=new DateTime(2014,9,11,19,5,22),newMsg=0},
                                new Contact{num="10001",lastTime=new DateTime(2014,9,10,12,7,16),newMsg=1},
                                new Contact{num="朱子沫",lastTime=DateTime.Now,newMsg=100}
                            };
        private void LoadContact(Contact[] ctt)
        {
            SortedDictionary<DateTime, cttLv> sctt = new SortedDictionary<DateTime, cttLv>();
            foreach (Contact t in ctt)
            {
                sctt.Add(t.lastTime, new cttLv(t.num, t.newMsg));
            }
            this.listView1.BeginUpdate();
            foreach (KeyValuePair<DateTime, cttLv> t in sctt)
            {
                string num_modified = NumModify(t.Value.num);
                this.listView1.Items.Add(t.Value.newMsg.ToString(), num_modified, t.Value.newMsg > 0 ? 1 : 0);
            }
            this.listView1.EndUpdate();
        }
        #endregion

        #region Functions
        //格式化联系人号码
        private string NumModify(string num)
        {
            string num_modified = "";
            num_modified += num;
            for (int i = 0; i < 30 - num.Length; i++)
            {
                num_modified += " ";
            }
            return num_modified;
        }
        //新消息接收
        private void newMessageUp(string from, DateTime time)
        {
            this.listView1.BeginUpdate();
            int tgtIndex = this.listView1.FindItemWithText(from).Index;
            int newMsg = int.Parse(this.listView1.Items[tgtIndex].Name) + 1;
            this.listView1.Items.RemoveAt(tgtIndex);
            this.listView1.Items.Insert(0, new ListViewItem(NumModify(from), 1));
            this.listView1.Items[0].Name = newMsg.ToString();
            this.listView1.EndUpdate();
        }

        //新信息
        private void client_RecvDatagram(object sender, UDP.NetEventArgs e)
        {
            this.listView1.BeginInvoke(new UDP.NetEventArgsHandler(RecvDatagram), new object[] { sender, e });
        }

        //获得短信后所做的事
        private void RecvDatagram(object sender, UDP.NetEventArgs e)
        {
            ServerMsg msg = new ServerMsg(e.Buff);

            if (msg.toName == myName)//防止同IP不同用户登录，理论上手机无需此判断
            {
                newMessageUp(msg.fromName, msg.sendTime);
            }
        }
        #endregion

        #region Test
        //
        private void button3_Click(object sender, EventArgs e)
        {
            newMessageUp("10086", DateTime.Now);
        }
        #endregion

    }
}
