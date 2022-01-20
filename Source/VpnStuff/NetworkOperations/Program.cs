using System;
using System.IO;

namespace NetworkOperations
{
    class Program
    {

        private static FileStream ostrm;
        private static StreamWriter writer;
        private static TextWriter oldOut = Console.Out;

        static void Main(string[] args)
        {

            //RedirectConsoleToLogFile();

            ConsoleTs.WriteLine("Querying IP info...");
            var (ip, ipInfo) = NetworkStuff.GetIpInfo();
            //ConsoleTs.WriteLine($"{ip} {ipInfo}");

            //ConsoleTs.WriteLine();

            ConsoleTs.WriteLine("Doing bitchute stuff...");
            using (var bitchute = new BitchuteStuff($"{ip} {ipInfo}"))
            {
                bitchute.DoStuff();
            }

            //ResetConsole();

        }

        private static void RedirectConsoleToLogFile()
        {
            oldOut = Console.Out;
            try
            {
                ostrm = new FileStream ("./Log.txt", FileMode.OpenOrCreate, FileAccess.Write);
                writer = new StreamWriter (ostrm);
            }
            catch (Exception e)
            {
                ConsoleTs.WriteLine ("Cannot open Redirect.txt for writing");
                ConsoleTs.WriteLine (e.Message);
                return;
            }
            Console.SetOut (writer);
            ConsoleTs.WriteLine ("Logging to file ON");

        }

        private static void ResetConsole()
        {
            ConsoleTs.WriteLine ("Logging to file OFF");
            Console.SetOut (oldOut);
            writer.Close();
            ostrm.Close();
            ConsoleTs.WriteLine ("Done");
        }
    }
}
