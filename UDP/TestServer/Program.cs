using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;

namespace TestServer
{
    static class Program
    {
      static  string[] Command;
      static UDP.Server server;

      volatile static int count = 0;

        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            while (true)
            {

                Write("");
                Command = Console.ReadLine().Split(' ');
                switch (Command[0].ToLower())
                {
                    case "start":
                        Start(Command);
                        break;
                    case "stop":
                        stop(Command);
                        break;
                    case "help":
                        help();
                        break;
                }

            }
        }


        static void Write(string content)
        {
            Console.Write(">>" + content);
        }


        static void Start(string[] param)
        {
            if (param.Length < 2)
            {
                Write("必须指定端口号\r\n");
                return;
            }
            UInt16 port;
            if(!UInt16.TryParse(param[1],out port))
            {
                Write("端口号应介于0-65535之间\r\n");
                return;
            }
            count = 0;
            Console.Clear();
            server = new UDP.Server(new IPEndPoint(IPAddress.Any, port));
            server.RecvDatagram += new UDP.NetEventArgsHandler(server_RecvDatagram);
            Write("服务启动成功,正在侦听端口 " + ((System.Net.IPEndPoint)server.userSocket.LocalEndPoint).Port.ToString() + "\r\n");
        }

        static void stop(string[] param)
        {
            if (server == null)
            {
                return;
            }
            server.Stop();
            server = null;

            Write("服务已停止.\r\n");
        }

        static void help()
        {
            Write("\tstart 启动服务\r\n\t\t语法:start 端口号\t如:start 2550\r\n\tstop 停止服务\r\n\t\t语法:stop\r\n");
        }


      static  void server_RecvDatagram(object sender, UDP.NetEventArgs e)
        {
            count += 1;
          //server.SendTo(e.Buff, e.RemoteEP);
          Console.WriteLine(string.Format("{0:D8}  接收到{1}的消息,内容:{2}",count, e.RemoteEP.ToString(), System.Text.Encoding.Unicode.GetString(e.Buff)));
         
        }
    }
}