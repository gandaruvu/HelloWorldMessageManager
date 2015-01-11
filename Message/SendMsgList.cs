using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading;

namespace Message
{
    public class SendMsgList
    {
        List<byte[]> msgByteList = new List<byte[]>();
        EndPoint ep;
        UDP.Server server;

        public SendMsgList(UDP.Server sv, List<byte[]> byteList, EndPoint ep)
        {
            msgByteList = byteList;
            this.ep = ep;
            server = sv;
        }

        public void Send()
        {
            foreach (var msgbytes in msgByteList)
            {
                server.SendTo(msgbytes, ep);
                Thread.Sleep(10);
            }
        }
    }
}
