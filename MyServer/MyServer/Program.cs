using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using L.Network;
using System.Threading;
using L.Logger;

namespace MyServer
{
    class Program
    {
        static void Main(string[] args)
        {
            ServerTest test = new ServerTest();
            test.Awake(NetworkProtocol.TCP, "127.0.0.1", 2000);
            while (true)
            {
                try
                {
                    Thread.Sleep(1);
                    test.Update();
                    //ObjectEvents.Instance.Update();
                }
                catch (Exception e)
                {
                    Log.Error(e.ToString());
                }
            }
        }
    }
}
