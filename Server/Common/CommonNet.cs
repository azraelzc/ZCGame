using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    public class CommonNet
    {
        public enum NetState
        {
            CLOSED,//主动断开
            CONNECTING,//正在连接
            CONNECTED,//连接成功
            DISCONNECTED,//被动断开
            TIMEOUT,//连接超时
            ERROR,//网络错误
        }
    }
}
