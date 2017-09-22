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

        public bool AddClient(Socket client)
        {
            Console.WriteLine("=======AddClient======{0}", client.GetHashCode());
            bool flag = false;
            if (!connectClients.ContainsKey(client.GetHashCode()))
            {
                connectClients.Add(client.GetHashCode(), client);
                flag = true;
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

        public bool RemoveClient(Socket client)
        {
            Console.WriteLine("=======RemoveClient======{0}", client.GetHashCode());
            bool flag = false;
            if (connectClients.ContainsKey(client.GetHashCode()))
            {
                connectClients.Remove(client.GetHashCode());
                flag = true;
            }
            if (sendMessPool.ContainsKey(client.GetHashCode()))
            {
                Queue<object> q = new Queue<object>();
                sendMessPool.Remove(client.GetHashCode());
            }
            if (reciveMessPool.ContainsKey(client.GetHashCode()))
            {
                Queue<object> q = new Queue<object>();
                reciveMessPool.Remove(client.GetHashCode());
            }
            return flag;
        }

        public Socket GetClientById(int id)
        {
            return connectClients[id];
        }

        public void PutSendMessageInPool(int clientCode,object obj)
        {
            Queue<object> q;
            if (sendMessPool.ContainsKey(clientCode))
            {
                q = sendMessPool[clientCode];
            }
            else
            {
                q = new Queue<object>();
                sendMessPool.Add(clientCode, q);
            }
            q.Enqueue(obj);
           
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
            }
            else
            {
                q = new Queue<object>();
                reciveMessPool.Add(clientCode, q);
            }
            q.Enqueue(obj);
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
    }

    public class ServerClass
    {
        public int id;
        public Queue<object> messages;
    }
}
