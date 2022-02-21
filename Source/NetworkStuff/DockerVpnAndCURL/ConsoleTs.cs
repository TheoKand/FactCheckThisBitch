using System;

namespace DockerVpnOrchestrator
{
    public static class ConsoleTs
    {

        public static void WriteLine(string log=null)
        {
            Console.WriteLine($"{DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss:fff")}: {log}");
        }
    }
}
