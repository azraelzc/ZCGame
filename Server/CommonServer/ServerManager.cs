using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Azrael.Common;

namespace Azrael.CommonServer
{
    class ServerManager
    {
        private static ServerManager mInstance = null;
        private ServerSocket socket = null;
        private MessagePool pool = null;
        public static ServerManager Instance
        {
            get
            {
                if (mInstance == null)
                {
                    mInstance = new ServerManager();
                    mInstance.Init();
                }
                return mInstance;
            }
        }
        public void Init()
        {
            socket = new ServerSocket();
            pool = new MessagePool();
        }

        public void C2B(uint id,string msg)
        {
            pool.PutSendMessageInPool(id,msg);
        }

        public void B2C(uint id, string msg)
        {
            pool.PutReciveMessageInPool(id, msg);
        }

        private void PushMessage()
        {
            Dictionary<uint, Queue<string>> map = pool.GetSendMap;
            List<uint> keys = new List<uint>(map.Keys);
            foreach (uint key in keys)
            {
                
            }

            map = pool.GetSendMap;
            keys = new List<uint>(map.Keys);
            foreach (uint key in keys)
            {
                Queue<string> q = map[key];
                if (q.Count > 0)
                {
                    while (q.Count > 0)
                    {
                        socket.SendMessage(q.Dequeue());
                    }
                }
                
            }
        }

        public void Update()
        {

        }
    }
}
