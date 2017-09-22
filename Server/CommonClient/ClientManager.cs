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

        public void Update()
        {
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
