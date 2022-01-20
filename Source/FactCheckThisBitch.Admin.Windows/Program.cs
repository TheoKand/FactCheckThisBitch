using FactCheckThisBitch.Admin.Windows.Forms;
using System;
using System.Text;
using System.Windows.Forms;

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
