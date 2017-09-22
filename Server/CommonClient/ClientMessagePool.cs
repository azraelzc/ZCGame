using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonClient
{
    public class ClientMessagePool
    {
        private Queue<object> sendQueue = new Queue<object>();
        private Queue<object> reciveQueue = new Queue<object>();

        public void PutSendMessageInPool(object obj)
        {
            sendQueue.Enqueue(obj);
        }

        public bool HasSendMessage
        {
            get
            {
                return sendQueue.Count > 0;
            }
        }

        public object GetSendMessage()
        {
            return sendQueue.Dequeue();
        }

        public void PutReciveMessageInPool(object obj)
        {
            reciveQueue.Enqueue(obj);
        }

        public bool HasReciveMessage
        {
            get
            {
                return reciveQueue.Count > 0;
            }
        }

        public object GetReciveMessage()
        {
            return reciveQueue.Dequeue();
        }
    }
}
