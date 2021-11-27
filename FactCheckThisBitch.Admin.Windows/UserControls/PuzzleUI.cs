using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using FactCheckThisBitch.Models;

namespace FactCheckThisBitch.Admin.Windows.UserControls
{
    public partial class PuzzleUI : UserControl
    {
        public Puzzle Puzzle;

        public PuzzleUI()
        {
            InitializeComponent();
        }
    }
}
