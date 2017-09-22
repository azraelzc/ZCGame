using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using System.Net.Sockets;
using CommonUnit;

namespace CommonServer
{
    public class ServerManager
    {
        public delegate void SendEvent(Socket client, object obj);
        private SendEvent mSendEvent;
        public event SendEvent OnSendEvent { add { mSendEvent += value; } remove { mSendEvent -= value; } }
        private static ServerManager mInstance = null;
        private ServerSocket socket = null;
        private ServerMessagePool pool = null;
        public static ServerManager Instance
        {
            get
            {
                if (mInstance == null)
                {
                    mInstance = new ServerManager();
                }
                return mInstance;
            }
        }
        public void Init()
        {
            pool = new ServerMessagePool();
            socket = new ServerSocket();
            socket.init();
        }

        public bool AddClient(Socket client)
        {
            return pool.AddClient(client);
        }

        public bool RemoveClient(Socket client)
        {
            return pool.RemoveClient(client);
        }

        public void C2B(int clientCode,object obj)
        {
            pool.PutSendMessageInPool(clientCode,obj);
        }

        public void B2C(int clientCode, object obj)
        {
            pool.PutReciveMessageInPool(clientCode,obj);
        }

        public void Update(int delta)
        {
            while (true)
            {
                ServerClass c = pool.HasSendMessages();
                if (c == null)
                {
                    break;
                }
                Queue<object> messages = c.messages;
                Socket client = pool.GetClientById(c.id);
                if (client != null)
                {
                    while(messages.Count > 0)
                    { 
                        mSendEvent.Invoke(client, messages.Dequeue());
                    }
                }           
            }

            while (true)
            {
                ServerClass c = pool.HasReciveMessages();
                if (c == null)
                {
                    break;
                }
                Queue<object> messages = c.messages;
                Socket client = pool.GetClientById(c.id);
                if (client != null)
                {
                    while (messages.Count > 0)
                    {
                        OnEvent(messages.Dequeue());
                    }
                }
            }
        }

        public void OnEvent(object obj)
        {
            Console.WriteLine("======OnEvent======{0}", obj);
            if (obj is Player)
            {
                Player p = obj as Player;
                Console.WriteLine("======OnEvent======{0}", p.ToString());
            }
            else if (obj is BuffEvent)
            {
                BuffEvent buff = obj as BuffEvent;
                Console.WriteLine("======OnEvent======{0}", buff.ToString());
            }
        }
    }
}
