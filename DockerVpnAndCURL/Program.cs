using System;
using System.Diagnostics;
using System.Linq;

namespace DockerVpnAndCURL
{
    class Program
    {

        private static string AllNordVpnCountries =
            "Albania, Argentina, Australia, Austria, Belgium, Bosnia_And_Herzegovina, Brazil, Bulgaria, Canada, Chile, Costa_Rica, Croatia, Cyprus, Czech_Republic, Denmark, Estonia, Finland, France, Georgia, Germany, Greece, Hong_Kong, Hungary, Iceland, India, Indonesia, Ireland, Israel, Italy, Japan, Latvia, Lithuania, Luxembourg, Malaysia, Mexico, Moldova, Netherlands, New_Zealand, North_Macedonia, Norway, Poland, Portugal, Romania, Serbia, Singapore, Slovakia, Slovenia, South_Africa, South_Korea, Spain, Sweden, Switzerland, Taiwan, Thailand, Turkey, Ukraine, United_Kingdom, United_States, Vietnam";


        private static string NordVpnCountries =
            "France, Germany, Netherlands, United_Kingdom, Canada";

        static void Main(string[] args)
        {
            var countries = NordVpnCountries.Split(",").Select(x => x.Trim()).ToList();

            for (int y = 0; y < 5; y++)
            {
                for (int i = 0; i < countries.Count; i++)
                {
                    var country = countries[i];
                    var countryVpnContainerName = $"vpn{country}";
                    var countryContainerName = country.ToLowerInvariant();

                    ConsoleTs.WriteLine(country);

                    RunCommand($"run -ti --cap-add=NET_ADMIN --name {countryVpnContainerName} -e CONNECT={country} -e USER=tkandiliotis@gmail.com  -e PASS=NordVpn123 -e TECHNOLOGY=NordLynx -d ghcr.io/bubuntux/nordvpn", 10);
                    var vpnLog = RunCommand($"logs {countryVpnContainerName}").NordVpnContainerLog();
                    ConsoleTs.WriteLine(vpnLog);

                    RunCommand(
                        $"run -it --net=container:{countryVpnContainerName} --name {countryContainerName} dockernetworkpoc", 4);
                    ConsoleTs.WriteLine(RunCommand($"logs {countryContainerName}"));

                    RunCommand($"stop {countryVpnContainerName}");
                    RunCommand($"stop {countryContainerName}");
                    RunCommand($"rm -v {countryVpnContainerName}");
                    RunCommand($"rm -v {countryContainerName}",30);
                }
            }


            ConsoleTs.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        private static string RunCommand(string command, int delayInSeconds = 1)
        {
            var output = "";

            var processInfo = new ProcessStartInfo("docker", $"{command}")
            {
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            using (var process = new Process())
            {
                process.StartInfo = processInfo;
                process.Start();
                output = process.StandardOutput.ReadToEnd();
                if (!process.HasExited)
                {
                    process.WaitForExit(-1);
                    output = process.StandardOutput.ReadToEnd();
                }

                if (process.ExitCode != 0)
                {
                    var error = $"Error in command {command} {process.StandardError.ReadToEnd()}";
                    throw new Exception(error);
                }
                process.Close();
            }

            System.Threading.Thread.Sleep(delayInSeconds * 1000);

            return output;

        }


    }


}
