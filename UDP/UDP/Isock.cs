using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace UDP
{

    public delegate void NetEventArgsHandler(object sender,NetEventArgs e);


    /// <summary>
    /// 套接字接口接口
    /// </summary>
    public interface Isock
    {

        event NetEventArgsHandler RecvDatagram;

        int SendTo(byte[] buff, EndPoint remoteEP);

        void Stop();
        bool ISStart { get;}
        Socket userSocket
        {
            get;
        }
        

    }


    public abstract class SocketsBase : Isock
    {
        Socket sock;
        IPEndPoint LocalEP;
        byte[] recvBuff;
        public event NetEventArgsHandler RecvDatagram;
        bool isStart;

        public SocketsBase(IPEndPoint ep)
        {
            LocalEP = ep;
            sock = new Socket(ep.AddressFamily, SocketType.Dgram, ProtocolType.Udp);
            sock.ReceiveBufferSize = UInt16.MaxValue * 20;
            sock.Bind(this.LocalEP);

            Listen();
            isStart = true;
        }

        #region Isock 成员

        public int SendTo(byte[] buff, EndPoint remoteEP)
        {
            try
            {
               return sock.SendTo(buff, remoteEP);
            }
            catch (SocketException ex)
            {
                
                throw (ex);
            }
        }

        private void Listen()
        {
            asynCallStateobject obj=new asynCallStateobject();
            obj.sock=this.sock;
            obj.ep=(EndPoint)new IPEndPoint(IPAddress.Any,0);
            lock (sock)
            {
                sock.BeginReceiveFrom(obj.buff, 0, obj.buff.Length, SocketFlags.None, ref obj.ep,
                    new AsyncCallback(endRecv), obj);
            }
        }

        protected void endRecv(IAsyncResult iar)
        {
            if (isStart)
            {
                try
                {
                    asynCallStateobject obj = (asynCallStateobject)iar.AsyncState;

                    int recv = obj.sock.EndReceiveFrom(iar, ref obj.ep);
                    recvBuff = new byte[recv];

                    Buffer.BlockCopy(obj.buff, 0, recvBuff, 0, recv);
                    onRecvDatagram(new NetEventArgs(recvBuff, (IPEndPoint)obj.ep));
                    Listen();
                }
                catch (SocketException ex)
                {
                    System.Diagnostics.Trace.WriteLine(ex.ToString());
                }
            }

        }

        public virtual void onRecvDatagram(NetEventArgs e)
        {
            if (this.RecvDatagram != null)
            {
                RecvDatagram(this, e);
            }
        }


        public void Stop()
        {
            if (sock == null)
            {
                return;
            }
            isStart = false;
            sock.Blocking = false;
            sock.Shutdown(SocketShutdown.Both);
            sock.Close();
            sock = null;
           
        }

        public Socket userSocket
        {
            get { return sock; }
        }

        public bool ISStart
        {
            get { return isStart; }
        }
        #endregion       
    
       
    
       
    
    }

    public class NetEventArgs : EventArgs
    {
        public NetEventArgs( byte[] buff,IPEndPoint ep)
        {
            this.buff = buff;
            this.ep = ep;
        }

        byte[] buff;        
        IPEndPoint ep;

        public byte[] Buff
        {
            get { return this.buff; }
        }

        public IPEndPoint RemoteEP
        {
            get { return ep; }
        }

    }

    internal class asynCallStateobject
    {

        public asynCallStateobject()
        {
            buff = new byte[65536];
        }
        public Socket sock;

        public byte[] buff;

        public EndPoint ep;
    }

    public class Client : SocketsBase
    {
        public Client(IPEndPoint ep)
            : base(ep)
        {
         
            
        }
    }

    public class Server : SocketsBase
    {
        public Server(IPEndPoint ep)
            : base(ep)
        { 
        
        }
    }
}
