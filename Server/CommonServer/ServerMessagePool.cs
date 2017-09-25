using Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace CommonServer
{
    //消息池和连接池放一起
    public class ServerMessagePool
    {
        
        private Dictionary<int, Queue<object>> sendMessPool = new Dictionary<int, Queue<object>>();
        private Dictionary<int, Queue<object>> reciveMessPool = new Dictionary<int, Queue<object>>();
        private Dictionary<int, Socket> connectClients = new Dictionary<int, Socket>();
        private Dictionary<int, int> pingList = new Dictionary<int, int>();//心跳包
        public Dictionary<int, int> GetPingList
        {
            get
            {
                return pingList;
            }
        }

        public bool AddClient(Socket client)
        {
            Console.WriteLine("===========AddClient{0}===========", client.GetHashCode());
            bool flag = false;
            if (!connectClients.ContainsKey(client.GetHashCode()))
            {
                connectClients.Add(client.GetHashCode(), client);
                flag = true;
            }
            if (!pingList.ContainsKey(client.GetHashCode()))
            {
                pingList.Add(client.GetHashCode(), 0);
            }
            if (!sendMessPool.ContainsKey(client.GetHashCode()))
            {
                Queue<object> q = new Queue<object>();
                sendMessPool.Add(client.GetHashCode(), q);
            }
            if (!reciveMessPool.ContainsKey(client.GetHashCode()))
            {
                Queue<object> q = new Queue<object>();
                reciveMessPool.Add(client.GetHashCode(), q);
            }
            return flag; 
        }

        public bool RemoveClient(int id)
        {
            Console.WriteLine("===========RemoveClient{0}===========", id);
            bool flag = false;
            if (connectClients.ContainsKey(id))
            {
                connectClients.Remove(id);
                flag = true;
            }
            if (pingList.ContainsKey(id))
            {
                pingList.Remove(id);
            }
            if (sendMessPool.ContainsKey(id))
            {
                sendMessPool.Remove(id);
            }
            if (reciveMessPool.ContainsKey(id))
            {
                reciveMessPool.Remove(id);
            }
            return flag;
        }

        public bool HasSocket(int id)
        {
            return connectClients.ContainsKey(id);
        }

        public Socket GetClientById(int id)
        {
            Socket socket = null;
            if (connectClients.ContainsKey(id))
            {
                socket = connectClients[id];
            }
            return socket;
        }

        public void PutSendMessageInPool(int clientCode,object obj)
        {
            Queue<object> q;
            if (sendMessPool.ContainsKey(clientCode))
            {
                q = sendMessPool[clientCode];
                q.Enqueue(obj);
            }
        }

        public ServerClass HasSendMessages()
        {
            ServerClass ret = null;
            List<int> key = new List<int>(sendMessPool.Keys);
            for (int i = 0; i < key.Count; i++)
            {
                if (sendMessPool[key[i]].Count > 0)
                {
                    ret = new ServerClass();
                    ret.id = key[i];
                    ret.messages = sendMessPool[key[i]];
                    break;
                }
            }
            return ret;
        }

        public void PutReciveMessageInPool(int clientCode,object obj)
        {
            Queue<object> q;
            if (reciveMessPool.ContainsKey(clientCode))
            {
                q = reciveMessPool[clientCode];
                q.Enqueue(obj);
            }
        }

        public ServerClass HasReciveMessages()
        {
            ServerClass ret = null;
            List<int> key = new List<int>(reciveMessPool.Keys);
            for (int i = 0; i < key.Count; i++)
            {
                if (reciveMessPool[key[i]].Count > 0)
                {
                    ret = new ServerClass();
                    ret.id = key[i];
                    ret.messages = reciveMessPool[key[i]];
                    break;
                }
            }
            return ret;
        }

        public void ResetPing(int id)
        {
            Console.WriteLine("=====ResetPing{0}====", id);
            if (pingList.ContainsKey(id))
            {
                pingList[id] = 0;
            }
        }
    }

    public class ServerClass
    {
        public int id;
        public Queue<object> messages;
    }
}
