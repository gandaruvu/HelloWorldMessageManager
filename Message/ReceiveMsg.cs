using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using UDP;
using System.Timers;

namespace Message
{
    public class ReceiveMsg : AllMsg
    {
        public DateTime sendTime;
        public string toName;
        public string fromName;
        public string message;
        public string messagel;
        private MsgType type;

        SortedDictionary<string, List<SendMsg>> msgs = new SortedDictionary<string, List<SendMsg>>();

        public ReceiveMsg(byte[] bytes)
        {
            string msg = Encoding.Unicode.GetString(bytes);
            string[] detail = msg.Split(this.joinSplit.ToArray(), StringSplitOptions.RemoveEmptyEntries);
            this.toName = detail[0];
            this.fromName = detail[1];
            this.sendTime = DateTime.Parse(detail[2]);
            this.message = detail[3];
            this.type = MsgType.Receive;
        }
    }
}
