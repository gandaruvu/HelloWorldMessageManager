using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using UDP;
using System.Timers;

namespace Message
{
    public class SendMsg : AllMsg
    {
        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="toName"></param>
        /// <param name="fromName"></param>
        /// <param name="sendTime">应当发送的时间</param>
        /// <param name="reqTime">发送给服务器的时间</param>
        /// <param name="message">combined message</param>
        public SendMsg(string toName, string fromName, DateTime sendTime, string message, string messagel)
        {
            this.toName = toName;
            this.fromName = fromName;
            this.sendTime = sendTime;
            this.message = message;
            this.messagel = messagel;
            this.message = CombineMsg();
            this.type = MsgType.Send;
        }
        public SendMsg(string toName, string fromName)
        {
            this.message = string.Join(this.joinSplit, new string[] { toName, fromName, DateTime.Now.ToString(), "LOGONMESSAGE" });
        }
        public void Send(UDP.Server server, EndPoint toEp)
        {
            SendMsg msg = new SendMsg(toName, fromName, sendTime, message, messagel);
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
