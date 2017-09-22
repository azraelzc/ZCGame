using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonLang.IO.Attribute
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class MessageTypeAttribute : System.Attribute
    {
        readonly int _messageTypeID;

        public MessageTypeAttribute(int messageType)
        {
            _messageTypeID = messageType;
        }
        public int MessageTypeID
        {
            get { return _messageTypeID; }
        }
    }
}
