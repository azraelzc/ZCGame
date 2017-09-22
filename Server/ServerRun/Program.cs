using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonServer;

namespace ServerRun
{
    class Program
    {
        static void Main(string[] args)
        {
            ServerManager.Instance.Init();
        }
    }
}
