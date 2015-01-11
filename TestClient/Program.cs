using System;
using System.Collections.Generic;
using System.Windows.Forms;
using GUI_main;

namespace TestClient
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Login login = new Login();
            Application.Run(login);
            if (!string.IsNullOrEmpty(login.userName))
            {
                Application.Run(new MainForm(login.userName));
            }
        }
    }
}