using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonClient
{
    public class ClientManager
    {
        public delegate void ReciveEvent(object obj);
        private ReciveEvent mReciveEvent;
        public event ReciveEvent OnReciveEvent{ add{mReciveEvent += value; } remove{mReciveEvent -= value; } }

        private static ClientManager mInstance = null;
        private ClientSocket socket = null;
        private ClientMessagePool pool = null;
        private int pingTime = 0;
        public static ClientManager Instance
        {
            get
            {
                if (mInstance == null)
                {
                    mInstance = new ClientManager();
                }
                return mInstance;
            }
        }

        public void Init(string ip, int port)
        {
            InitSocket(ip, port);
            InitMessgePool();
        }

        public void InitSocket(string ip, int port)
        {
            socket = new ClientSocket();
            socket.ConnectServer(ip, port);
        }

        public void InitMessgePool()
        {
            pool = new ClientMessagePool();
        }

        public void B2C(object obj)
        {
            pool.PutSendMessageInPool(obj);
        }

        public void C2B(object obj)
        {
            pool.PutReciveMessageInPool(obj);
        }

        public void SendPing(int delta)
        {
            Console.WriteLine("========SendPing{0}=======", pingTime);
            pingTime += delta;
            if (pingTime >= PublicConstants.PING_INTERVAL_TIMEMS)
            {
                pingTime = 0;
                B2C(new PingClass());
            }
        }

        public void Update(int delta)
        {
            SendPing(delta);
            while (pool.HasSendMessage)
            {
                object obj = pool.GetSendMessage();
                socket.SendMessage(obj);
            }

            while (pool.HasReciveMessage)
            {
                object obj = pool.GetReciveMessage();
                mReciveEvent.Invoke(obj);
            }
        }
    }
}
