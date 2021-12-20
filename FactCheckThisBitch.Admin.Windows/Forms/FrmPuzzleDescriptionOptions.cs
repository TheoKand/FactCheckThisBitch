using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FackCheckThisBitch.Common;
using FactCheckThisBitch.Models;

namespace FactCheckThisBitch.Admin.Windows.Forms
{
    public partial class FrmPuzzleDescriptionOptions : Form
    {

        public PuzzleDescriptionOptions Options = new PuzzleDescriptionOptions();

        public FrmPuzzleDescriptionOptions()
        {
            InitializeComponent();
            InitForm();
        }


        private void InitForm()
        {
            lstLeet.DataSource = Enum.GetValues(typeof(Level)).Cast<Level>();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Options.LeetLevel =  (Level) Enum.Parse(typeof(Level), lstLeet.SelectedValue.ToString() ?? string.Empty);
            Options.IncludeDescriptions = chkIncludeDescriptions.Checked;
            Options.IncludeReferenceTitles = chkIncludeReferenceTitles.Checked;
            DialogResult = DialogResult.OK;
            Close();
        }
    }

    public class PuzzleDescriptionOptions
    {
        public bool IncludeDescriptions;
        public bool IncludeReferenceTitles;
        public Level LeetLevel;
    }
}