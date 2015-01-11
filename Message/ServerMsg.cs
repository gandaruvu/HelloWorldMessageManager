using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using UDP;
using System.Timers;

namespace Message
{
    public class ServerMsg
    {
        public DateTime sendTime;
        public string toName;
        public string fromName;
        public string message;

        string splitStr = "|||";
        public ServerMsg(byte[] bytes)
        {
            string msg = Encoding.Unicode.GetString(bytes);
            string[] detail = msg.Split(splitStr.ToArray());
            this.toName = detail[0];
            this.fromName = detail[1];
            this.sendTime = DateTime.Parse(detail[2]);
            this.message = detail[3];
        }

        public void Send(UDP.Server server,EndPoint toEp)
        {
            ClientMsg msg = new ClientMsg(toName, fromName, sendTime, message);
            if (DateTime.Now >= sendTime)
            {
                msg.SendMsgToClient(server, toEp);
            }
            else
            {
                Timer newTimer = new Timer();   
            }
        }

    }
}
