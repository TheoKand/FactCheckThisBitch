using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace DockerVpnAndCURL
{
    class Program
    {

        private static string AllNordVpnCountries =
            "Albania, Argentina, Australia, Austria, Belgium, Bosnia_And_Herzegovina, Brazil, Bulgaria, Canada, Chile, Costa_Rica, Croatia, Cyprus, Czech_Republic, Denmark, Estonia, Finland, France, Georgia, Germany, Greece, Hong_Kong, Hungary, Iceland, India, Indonesia, Ireland, Israel, Italy, Japan, Latvia, Lithuania, Luxembourg, Malaysia, Mexico, Moldova, Netherlands, New_Zealand, North_Macedonia, Norway, Poland, Portugal, Romania, Serbia, Singapore, Slovakia, Slovenia, South_Africa, South_Korea, Spain, Sweden, Switzerland, Taiwan, Thailand, Turkey, Ukraine, United_Kingdom, United_States, Vietnam";

        private static string NordVpnCountries =
            "United_Kingdom,United_Kingdom,France, Germany, Netherlands, United_Kingdom, Canada,Austria,Denmark,Finland";

        //private static string NordVpnCountries =
        //    "Denmark";

        static void Main(string[] args)
        {
            var countries = NordVpnCountries.Split(",").Select(x => x.Trim()).ToList();

            var initialDelayMinutes = 40;
            Thread.Sleep(initialDelayMinutes * 1000);

            while(true)
            {
                var actions = new List<Action>();
                for (int i = 0; i < countries.Count; i++)
                {
                    var index = i;
                    var country = countries[index];
                    var countryVpnContainerName = $"vpn{country}{index}";
                    var countryContainerName = $"{country.ToLowerInvariant()}{index}";

                    var action = new Action(() =>
                    {
                        ConsoleTs.WriteLine($"{index}:{country}");

                        Thread.Sleep(index * 1000);

                        RunCommand($"run -ti --cap-add=NET_ADMIN --name {countryVpnContainerName} -e CONNECT={country} -e USER=tkandiliotis@gmail.com  -e PASS=NordVpn123 -e TECHNOLOGY=NordLynx -d ghcr.io/bubuntux/nordvpn", 30);
                        var vpnLog = RunCommand($"logs {countryVpnContainerName}").NordVpnContainerLog();
                        ConsoleTs.WriteLine($"{index}:{country}:{vpnLog}");

                        RunCommand(
                            $"run -it --net=container:{countryVpnContainerName} --name {countryContainerName} dockernetworkpoc", 10);
                        ConsoleTs.WriteLine($"{index}:{country}:{RunCommand($"logs {countryContainerName}")}");

                        RunCommand($"stop {countryVpnContainerName}");
                        RunCommand($"stop {countryContainerName}", 5);

                        RunCommand($"rm -v {countryVpnContainerName}");
                        RunCommand($"rm -v {countryContainerName}");

                    });
                    actions.Add(action);
                }

                SpawnAndWait(actions);

                actions.Clear();

                //6/1/2022 16:40 too many requests. try again in half to one hour.
                int minutesToWait = 61;
                ConsoleTs.WriteLine($"Waiting {minutesToWait} minutes..." );
                Thread.Sleep(minutesToWait * 60 * 1000);
            }


            ConsoleTs.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        private static void SpawnAndWait(IEnumerable<Action> actions)
        {
            var list = actions.ToList();
            var handles = new ManualResetEvent[actions.Count()];
            for (var i = 0; i < list.Count; i++)
            {
                handles[i] = new ManualResetEvent(false);
                var currentAction = list[i];
                var currentHandle = handles[i];
                Action wrappedAction = () => { try { currentAction(); } finally { currentHandle.Set(); } };
                ThreadPool.QueueUserWorkItem(x => wrappedAction());
            }

            WaitHandle.WaitAll(handles);
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
                    process?.Close();
                    return error;
                }
                process.Close();
            }

            Thread.Sleep(delayInSeconds * 1000);

            return output;

        }


    }


}
