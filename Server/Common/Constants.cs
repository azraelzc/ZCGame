using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    public class PublicConstants
    {
        //ping包间隔时间
        public const int PING_INTERVAL_TIMEMS = 1000;
        //丢失ping包断开连接时间
        public const int PING_LOST_TIMEMS = 5000;
    }
}
