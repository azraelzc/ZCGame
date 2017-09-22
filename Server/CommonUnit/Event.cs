using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonUnit
{
    [Serializable]
    public class Event
    {
        public uint uid;
        public int templateId;
    }

    [Serializable]
    public class BuffEvent:Event
    {
        public override string ToString()
        {
            return "你中了个buff"+ templateId;
        }
    }
}
