using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Azrael.Common
{
    public class MessagePool
    {
        private Dictionary<uint, Queue<string>> sendMessageMap = new Dictionary<uint, Queue<string>>();

        public Dictionary<uint, Queue<string>> GetSendMap
        {
            get
            {
                return sendMessageMap;
            }
        }

        private Dictionary<uint, Queue<string>> ReciveMessageMap = new Dictionary<uint, Queue<string>>();

        public Dictionary<uint, Queue<string>> GetReciveMap
        {
            get
            {
                return ReciveMessageMap;
            }
        }

        //将发送出去的消息放入发送消息池
        public void PutSendMessageInPool(uint id, string msg)
        {
            Queue<string> messages;

            if (sendMessageMap.ContainsKey(id))
            {
                sendMessageMap.TryGetValue(id, out messages);
            }
            else
            {
                messages = new Queue<string>();
                sendMessageMap.Add(id, messages);
            }
            messages.Enqueue(msg);
        }


        //将接受的消息放入接收消息池
        public void PutReciveMessageInPool(uint id, string msg)
        {
            Queue<string> messages;

            if (ReciveMessageMap.ContainsKey(id))
            {
                ReciveMessageMap.TryGetValue(id, out messages);
            }
            else
            {
                messages = new Queue<string>();
                ReciveMessageMap.Add(id, messages);
            }
            messages.Enqueue(msg);
        }
    }
}
