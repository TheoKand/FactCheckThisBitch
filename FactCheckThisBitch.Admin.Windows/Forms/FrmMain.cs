using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FactCheckThisBitch.Admin.Windows.Forms;
using FactCheckThisBitch.Models;

namespace FactCheckThisBitch.Admin.Windows
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            FrmPuzzle puzzleForm = new FrmPuzzle(new Puzzle()
            {
                Title = "Are Covid Vaccines Really Vaccines?",
                Thesis = "hah ahahahaha",
                Width = 4,
                Height = 4
            });
            puzzleForm.Show();

            //FrmPuzzle puzzleForm = new FrmPuzzle( null);
            //puzzleForm.Show();
        }
    }
}
