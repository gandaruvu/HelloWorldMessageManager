using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace TestClient
{
    public partial class Login : Form
    {
        public string userName = "";
        public Login()
        {
            InitializeComponent();
        }

        //窗口拖拽
        Point mousePos;
        bool leftDown;
        bool button_over;
        private void Login_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && !button_over)
            {
                mousePos = new Point(-e.X, -e.Y); //得到变量的值
                leftDown = true;                  //点击左键按下时标注为true;
            }
        }

        private void Login_MouseMove(object sender, MouseEventArgs e)
        {
            if (leftDown)
            {
                Point mouseSet = Control.MousePosition;
                mouseSet.Offset(mousePos.X, mousePos.Y);  //设置移动后的位置
                Location = mouseSet;
            }
        }

        private void Login_MouseUp(object sender, MouseEventArgs e)
        {
            if (leftDown)
            {
                leftDown = false;
            }
        }

        //最小化按钮//
        private void button2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void button2_MouseDown(object sender, MouseEventArgs e)
        {
            this.button2.BackgroundImage = global::TestClient.Properties.Resources.mini_down;
        }

        private void button2_MouseHover(object sender, EventArgs e)
        {
            button_over = true;
            this.button2.BackgroundImage = global::TestClient.Properties.Resources.mini_over;
        }

        private void button2_MouseLeave(object sender, EventArgs e)
        {
            button_over = false;
            this.button2.BackgroundImage = global::TestClient.Properties.Resources.mini;
        }

        //关闭按钮 //     
        private void button3_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button3_MouseHover(object sender, EventArgs e)
        {
            button_over = true;
            this.button3.BackgroundImage = global::TestClient.Properties.Resources.close_over;
        }


        private void button3_MouseLeave(object sender, EventArgs e)
        {
            button_over = false;
            this.button3.BackgroundImage = global::TestClient.Properties.Resources.close;
        }

        private void button3_MouseDown(object sender, MouseEventArgs e)
        {
            this.button3.BackgroundImage = global::TestClient.Properties.Resources.close_down;
        }

        private void login()
        {
            string name = AccountNameBox.Text;
            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("请输入用户名");
                return;
            }
            this.userName = name;
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            login();
        }

        private void button1_MouseDown(object sender, MouseEventArgs e)
        {
            this.button1.BackgroundImage = global::TestClient.Properties.Resources.btn_login_down;
        }

        private void button1_MouseHover(object sender, EventArgs e)
        {
            this.button1.BackgroundImage = global::TestClient.Properties.Resources.btn_login_over;
        }

        private void button1_MouseLeave(object sender, EventArgs e)
        {
            this.button1.BackgroundImage = global::TestClient.Properties.Resources.btn_login;
        }

        private void AccountNameBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13 && AccountNameBox.Text.Length > 0)
            {
                this.button1.BackgroundImage = global::TestClient.Properties.Resources.btn_login_down;
            }
        }

        private void AccountNameBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13 && AccountNameBox.Text.Length > 0)
            {
                this.button1.BackgroundImage = global::TestClient.Properties.Resources.btn_login;
                login();
            }
        }

        private void AccountNameBox_TextChanged(object sender, EventArgs e)
        {
            if (this.AccountNameBox.Text.Length == 0)
            {
                button1.Enabled = false;
                this.button1.BackgroundImage = global::TestClient.Properties.Resources.btn_login_dis;
            }
            else
            {
                button1.Enabled = true;
                this.button1.BackgroundImage = global::TestClient.Properties.Resources.btn_login;
            }
        }

    }
}
