using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonUnit
{
    [Serializable]
    public class Player
    {
        public uint id;
        public string name;
        public int force;

        public int OnHit()
        {
            int result = 0;
            return result;
        }

        public override string ToString()
        {
            return "id=" + id + ",name=" + name + ",force=" + force;
        }
    }
}
