using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using UDP;

namespace Message
{
    enum MsgType
    {
        Send = 0,
        Receive = 1,
        Draft = 2
    }
    public class ClientMsg
    {
        EndPoint serverEP = new System.Net.IPEndPoint(IPAddress.Parse("127.0.0.1"), int.Parse("11111"));

        public string toName;
        public string fromName;
        public DateTime sendTime;
        public string message;

        private MsgType type;

        private string joinSplit = "|||";

        public ClientMsg(string fromName)
        {
            this.message = "LOGONMESSAGE";
        }
        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="toName"></param>
        /// <param name="fromName"></param>
        /// <param name="sendTime">应当发送的时间</param>
        /// <param name="reqTime">发送给服务器的时间</param>
        /// <param name="message">combined message</param>
        public ClientMsg(string toName,string fromName,DateTime sendTime,string message)
        {
            this.toName = toName;
            this.fromName = fromName;
            this.sendTime = sendTime;
            this.message = CombineMsg();
            this.type = MsgType.Send;
        }

        public void SendMsgToServer(UDP.Client client)
        {
            client.SendTo(System.Text.Encoding.Unicode.GetBytes(this.message),serverEP);
        }
        public void SendMsgToClient(UDP.Server client,EndPoint clientEP)
        {
            client.SendTo(System.Text.Encoding.Unicode.GetBytes(this.message), clientEP);
        }
        private string CombineMsg()
        {
            string[] CombineArray = { this.toName, this.fromName, this.sendTime.ToString(),this.message};
            return string.Join(joinSplit, CombineArray);
        }
    }
}
