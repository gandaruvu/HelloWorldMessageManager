using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace Message
{
    public enum MsgType
    {
        Send = 0,
        Receive = 1,
        Draft = 2
    }
    public class AllMsg
    {
        public string joinSplit = "|||";
        EndPoint serverEP = new System.Net.IPEndPoint(IPAddress.Parse("127.0.0.1"), int.Parse("11111"));

        public string toName;
        public string fromName;
        public DateTime sendTime;
        public string message;
        public string messagel;
        public MsgType type;

        public AllMsg()
        { }

        public void SendMsgToServer(UDP.Client client)
        {
            client.SendTo(System.Text.Encoding.Unicode.GetBytes(this.message), serverEP);
        }
        public void SendMsgToClient(UDP.Server client, EndPoint clientEP)
        {
            client.SendTo(System.Text.Encoding.Unicode.GetBytes(this.message), clientEP);
        }

        public string CombineMsg()
        {
            string[] CombineArray = { this.toName, this.fromName, this.sendTime.ToString(), this.message };
            return string.Join(joinSplit, CombineArray);
        }
    }
}
