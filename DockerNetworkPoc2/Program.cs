using System;

namespace DockerNetworkPoc2
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Querying IP info...");

            var (ip, ipInfo) = NetworkStuff.GetIpInfo();

            Console.WriteLine($"{ip} {ipInfo}");
        }
    }
}
