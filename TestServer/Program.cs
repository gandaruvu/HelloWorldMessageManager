using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using Message;
using User;
using System.Threading;

namespace TestServer
{
    static class Program
    {
        static string Command;
        static UDP.Server server;
        static int port = 11111;
        volatile static int count = 0;
        static ServerUser users = new ServerUser();
        static string LogonKey = "LOGONMESSAGE";
        volatile static SortedDictionary<string, List<byte[]>> msgs = new SortedDictionary<string, List<byte[]>>();

        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            while (true)
            {

                Write("");
                Command = Console.ReadLine();
                switch (Command.ToLower())
                {
                    case "start":
                        Start();
                        break;
                    case "stop":
                        stop();
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


        static void Start()
        {
            count = 0;
            Console.Clear();
            server = new UDP.Server(new IPEndPoint(IPAddress.Any, port));
            server.RecvDatagram += new UDP.NetEventArgsHandler(server_RecvDatagram);
            Write("服务启动成功,正在侦听端口 " + ((System.Net.IPEndPoint)server.userSocket.LocalEndPoint).Port.ToString() + "\r\n");
        }

        static void stop()
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
            Write("start 启动服务\r\nstop 停止服务\r\n");
        }

        static void server_RecvDatagram(object sender, UDP.NetEventArgs e)
        {
            ReceiveMsg msg = new ReceiveMsg(e.Buff);
            if (msg.message == LogonKey)
            {
                EndPoint remoteEP = e.RemoteEP;
                users.LogOn(msg.fromName,e.RemoteEP);
                if (msgs.ContainsKey(msg.fromName))
                {
                    SendMsgList msgList = new SendMsgList(server, msgs[msg.fromName], remoteEP);
                    Thread send = new Thread(new ThreadStart(msgList.Send));
                    send.Start();
                    msgs.Remove(msg.fromName);
                }
            }
            else
            {
                if (!msgs.ContainsKey(msg.toName))
                {
                    msgs.Add(msg.toName, new List<byte[]> { e.Buff });
                }
                else
                {
                    msgs[msg.toName].Add(e.Buff);
                }
                EndPoint toUserEP = users.CheckLog(msg.toName);
                if (toUserEP != null)
                {
                    SendMsg CheckLogMsg = new SendMsg(msg.toName,"Server");
                    CheckLogMsg.SendMsgToClient(server, toUserEP);
                }
            }
            Console.WriteLine(string.Format("接收到{0}发给{1}的消息,内容:{2}", msg.fromName, msg.toName, msg.message));
        }


    }
}