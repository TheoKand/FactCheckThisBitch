using System;
using System.Collections.Generic;
using System.Text;

namespace DockerNetworkPoc2
{
    public static class ConsoleTs
    {
        public static void WriteLine(string log=null)
        {
            Console.WriteLine($"{DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss:fff")}: {log}");
        }
    }
}
