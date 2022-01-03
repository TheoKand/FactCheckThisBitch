using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace DockerVpnAndCURL
{
    class Program
    {

        private static string AllNordVpnCountries =
            "Albania, Argentina, Australia, Austria, Belgium, Bosnia_And_Herzegovina, Brazil, Bulgaria, Canada, Chile, Costa_Rica, Croatia, Cyprus, Czech_Republic, Denmark, Estonia, Finland, France, Georgia, Germany, Greece, Hong_Kong, Hungary, Iceland, India, Indonesia, Ireland, Israel, Italy, Japan, Latvia, Lithuania, Luxembourg, Malaysia, Mexico, Moldova, Netherlands, New_Zealand, North_Macedonia, Norway, Poland, Portugal, Romania, Serbia, Singapore, Slovakia, Slovenia, South_Africa, South_Korea, Spain, Sweden, Switzerland, Taiwan, Thailand, Turkey, Ukraine, United_Kingdom, United_States, Vietnam";


        private static string NordVpnCountries =
            "France, Germany, Netherlands, United_Kingdom, Belgium, Canada";

        static void Main(string[] args)
        {
            var countries = NordVpnCountries.Split(",").Select(x => x.Trim()).ToList();

            for (int y = 0; y < 5; y++)
            {
                for (int i = 0; i < countries.Count; i++)
                {
                    var country = countries[i];

                    Console.WriteLine(country);

                    RunCommand(
                        $"run -ti --cap-add=NET_ADMIN --name vpn{country} -e CONNECT={country} -e USER=tkandiliotis@gmail.com  -e PASS=NordVpn123 -e TECHNOLOGY=OpenVPN -d ghcr.io/bubuntux/nordvpn");

                    System.Threading.Thread.Sleep(10  * 1000);

                    RunCommand(
                        $"run -it --net=container:vpn{country} --name dockernetworkpoc{country} dockernetworkpoc");
                
                    System.Threading.Thread.Sleep(12 * 1000);

                    RunCommand($"stop vpn{country}");
                    RunCommand($"stop browser{country}");
                    RunCommand($"rm -v vpn{country}");
                    RunCommand($"rm -v browser{country}");

                    System.Threading.Thread.Sleep(5 * 1000);
                }
            }


            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        private static void IncreaseViewCount(int pauseSeconds)
        {
            Console.Write("Restarting vpn...");
            RunCommand("restart vpn");
            System.Threading.Thread.Sleep(pauseSeconds * 1000);
            Console.Write("Viewing...");
            RunCommand("restart browser");
            Console.Write("OK");

        }



        private static void RunCommand(string command)
        {
            var processInfo = new ProcessStartInfo("docker", $"{command}")
            {
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };
            using var process = new Process();
            process.StartInfo = processInfo;
            var started = process.Start();

            process.WaitForExit(12000);
            if (!process.HasExited)
            {
                process.Kill();
            }
            var exitCode = process.ExitCode;
            process.Close();
        }


    }


}
