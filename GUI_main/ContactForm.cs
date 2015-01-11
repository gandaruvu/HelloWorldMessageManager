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
        private string userName = "";
        UDP.Client client;
        public ContactForm(string user)
        {
            InitializeComponent();
            client = new UDP.Client(new System.Net.IPEndPoint(IPAddress.Any, 0));
            client.RecvDatagram += new UDP.NetEventArgsHandler(client_RecvDatagram);
            loadContact(ctt_dummy);
            label1.Text = user;
            userName = user;
        }

        Point mousePos;
        bool leftDown;

        #region 联系人
        private ListViewItem selected;
        private struct contact
        {
            public string num;
            public DateTime lastTime;
            public int newMsg;
        }
        private struct ctt_lv
        {
            public string num;
            public int newMsg;
            public ctt_lv(string nnum, int nnewMsg)
            {
                num = nnum;
                newMsg = nnewMsg;
            }
        }
        contact[] ctt_dummy ={  new contact{num="10010",lastTime=new DateTime(2014,9,11,9,20,31),newMsg=0},
                                new contact{num="10086",lastTime=new DateTime(2014,9,11,19,5,22),newMsg=0},
                                new contact{num="10001",lastTime=new DateTime(2014,9,10,12,7,16),newMsg=1},
                                new contact{num="朱子沫",lastTime=DateTime.Now,newMsg=100}
                            };//模拟数据
        //
        //读入联系人数据
        //
        private void loadContact(contact[] ctt)
        {
            SortedDictionary<DateTime, ctt_lv> sctt = new SortedDictionary<DateTime, ctt_lv>();
            foreach (contact t in ctt)
            {
                sctt.Add(t.lastTime, new ctt_lv(t.num, t.newMsg));
            }
            this.listView1.BeginUpdate();
            foreach (KeyValuePair<DateTime, ctt_lv> t in sctt)
            {
                string num_modified = NumModify(t.Value.num);
                this.listView1.Items.Add(t.Value.newMsg.ToString(), num_modified, t.Value.newMsg > 0 ? 1 : 0);
            }
            this.listView1.EndUpdate();
        }

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
        private bool anyItemSelected;
        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (ListViewItem item in this.listView1.SelectedItems)
            {
                anyItemSelected = true;
                item.BackColor = Color.FromArgb(250, 250, 150);
                item.ImageIndex = int.Parse(item.Name) > 0 ? 3 : 2;
                if (selected != null && selected != item)
                {
                    selected.BackColor = Color.FromArgb(232, 232, 228);
                    selected.ImageIndex = int.Parse(selected.Name) > 0 ? 1 : 0;
                }
                selected = this.listView1.FocusedItem;
                this.listView1.FocusedItem.Selected = false;
            }
        }

        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {

        }

        private void listView1_MouseDown(object sender, MouseEventArgs e)
        {
            anyItemSelected = false;
        }

        private void listView1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && selected != null && anyItemSelected == false)
            {
                selected.BackColor = Color.FromArgb(232, 232, 228);
                selected.ImageIndex = int.Parse(selected.Name) > 0 ? 1 : 0;
            }
            else
            {

            }
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            MessageBox.Show(selected.Name);
        }
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
        #endregion

        #region 窗体
        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            this.label1.Focus();
            if (e.Button == MouseButtons.Left)
            {
                mousePos = new Point(-e.X, -e.Y);
                leftDown = true;
            }
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (leftDown)
            {
                Point mouseSet = Control.MousePosition;
                mouseSet.Offset(mousePos.X, mousePos.Y);
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
        #endregion

        #region 关闭按钮
        private void buttonClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void buttonClose_MouseHover(object sender, EventArgs e)
        {
            this.buttonClose.BackgroundImage = global::GUI_main.Properties.Resources.close_over;
        }


        private void buttonClose_MouseLeave(object sender, EventArgs e)
        {
            this.buttonClose.BackgroundImage = global::GUI_main.Properties.Resources.close;
        }

        private void buttonClose_MouseDown(object sender, MouseEventArgs e)
        {
            this.buttonClose.BackgroundImage = global::GUI_main.Properties.Resources.close_down;
        }
        #endregion

        #region 最小化按钮
        private void buttonMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void buttonMinimize_MouseDown(object sender, MouseEventArgs e)
        {
            this.buttonMinimize.BackgroundImage = global::GUI_main.Properties.Resources.mini_down;
        }

        private void buttonMinimize_MouseHover(object sender, EventArgs e)
        {
            this.buttonMinimize.BackgroundImage = global::GUI_main.Properties.Resources.mini_over;
        }

        private void buttonMinimize_MouseLeave(object sender, EventArgs e)
        {
            this.buttonMinimize.BackgroundImage = global::GUI_main.Properties.Resources.mini;
        }
        #endregion

        #region 测试
        private void button3_Click(object sender, EventArgs e)
        {
            newMessageUp("10086", DateTime.Now);
        }//模拟收信按钮
        #endregion

        #region 搜索按钮
        private void buttonSearch_Click(object sender, EventArgs e)
        {

        }

        private void buttonSearch_MouseDown(object sender, MouseEventArgs e)
        {
            this.buttonSearch.BackgroundImage = global::GUI_main.Properties.Resources.search_down;
        }

        private void buttonSearch_MouseHover(object sender, EventArgs e)
        {
            this.buttonSearch.BackgroundImage = global::GUI_main.Properties.Resources.search_over;
        }

        private void buttonSearch_MouseLeave(object sender, EventArgs e)
        {
            this.buttonSearch.BackgroundImage = global::GUI_main.Properties.Resources.search;
        }

        private void buttonSearch_MouseUp(object sender, MouseEventArgs e)
        {
            this.buttonSearch.BackgroundImage = global::GUI_main.Properties.Resources.search_over;
        }
        #endregion

        #region 新建联系人按钮
        private void buttonAdd_Click(object sender, EventArgs e)
        {

        }

        private void buttonAdd_MouseDown(object sender, MouseEventArgs e)
        {
            this.buttonAdd.BackgroundImage = global::GUI_main.Properties.Resources.add_down;
        }

        private void buttonAdd_MouseHover(object sender, EventArgs e)
        {
            this.buttonAdd.BackgroundImage = global::GUI_main.Properties.Resources.add_over;
        }

        private void buttonAdd_MouseLeave(object sender, EventArgs e)
        {
            this.buttonAdd.BackgroundImage = global::GUI_main.Properties.Resources.add;
        }

        private void buttonAdd_MouseUp(object sender, MouseEventArgs e)
        {
            this.buttonAdd.BackgroundImage = global::GUI_main.Properties.Resources.add_over;
        }
        #endregion

        #region 写信按钮
        private void buttonMail_Click(object sender, EventArgs e)
        {

        }

        private void buttonMail_MouseDown(object sender, MouseEventArgs e)
        {
            this.buttonMail.BackgroundImage = global::GUI_main.Properties.Resources.mail_down;
        }

        private void buttonMail_MouseHover(object sender, EventArgs e)
        {
            this.buttonMail.BackgroundImage = global::GUI_main.Properties.Resources.mail_over;
        }

        private void buttonMail_MouseLeave(object sender, EventArgs e)
        {
            this.buttonMail.BackgroundImage = global::GUI_main.Properties.Resources.mail;
        }

        private void buttonMail_MouseUp(object sender, MouseEventArgs e)
        {
            this.buttonMail.BackgroundImage = global::GUI_main.Properties.Resources.mail_over;
        }
        #endregion

        #region 搜索框
        private void textBox1_Enter(object sender, EventArgs e)
        {
            this.textBox1.Text = "";
            this.textBox1.ForeColor = Color.Black;
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            if (this.textBox1.Text == "")
                this.textBox1.Text = "请输入要搜索的短信内容...";
            this.textBox1.ForeColor = Color.Gray;
        }
        #endregion

        #region 用户名
        private void label1_MouseDown(object sender, MouseEventArgs e)
        {
            this.label1.Focus();
            if (e.Button == MouseButtons.Left)
            {
                mousePos = new Point(-e.X, -e.Y);
                leftDown = true;
            }
        }

        private void label1_MouseMove(object sender, MouseEventArgs e)
        {
            if (leftDown)
            {
                Point mouseSet = Control.MousePosition;
                mouseSet.Offset(mousePos.X, mousePos.Y - 26);
                Location = mouseSet;
            }
        }

        private void label1_MouseUp(object sender, MouseEventArgs e)
        {
            if (leftDown)
            {
                leftDown = false;
            }
        }
        #endregion

        #region 接收信息
        //新信息
        private void client_RecvDatagram(object sender, UDP.NetEventArgs e)
        {
            this.listView1.BeginInvoke(new UDP.NetEventArgsHandler(RecvDatagram), new object[] { sender, e });
        }

        //获得短信后所做的事
        private void RecvDatagram(object sender, UDP.NetEventArgs e)
        {
            ReceiveMsg msg = new ReceiveMsg(e.Buff);

            if (msg.toName == userName)//防止同IP不同用户登录，理论上手机无需此判断
            {
                newMessageUp(msg.fromName, msg.sendTime);
            }
        }
        #endregion








    }
}
