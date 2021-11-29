using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using FactCheckThisBitch.Admin.Windows.Forms;
using FactCheckThisBitch.Models;
using Newtonsoft.Json;

namespace FactCheckThisBitch.Admin.Windows
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FrmPuzzle());
        }
    }
}
