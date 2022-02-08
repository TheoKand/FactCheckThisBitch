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
            LoadForm();
        }

        private void InitForm()
        {
            lstLeet.DataSource = Enum.GetValues(typeof(Level)).Cast<Level>();
        }

        private void LoadForm()
        {
            if (!UserSettings.Instance().PuzzleDescriptionOptionsLeetLevel.IsEmpty())
            {
                lstLeet.SelectedItem = UserSettings.Instance().PuzzleDescriptionOptionsLeetLevel;
            }

            chkIncludeDescriptions.Checked = UserSettings.Instance().PuzzleDescriptionOptionsIncludeDescriptions;
            chkIncludeReferenceTitles.Checked = UserSettings.Instance().PuzzleDescriptionOptionsIncludeReferenceTitles;
        }

        private void SaveForm()
        {
            UserSettings.Instance().PuzzleDescriptionOptionsLeetLevel = lstLeet.SelectedValue.ToString();
            UserSettings.Instance().PuzzleDescriptionOptionsIncludeDescriptions = chkIncludeDescriptions.Checked;
            UserSettings.Instance().PuzzleDescriptionOptionsIncludeReferenceTitles = chkIncludeReferenceTitles.Checked;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Options.LeetLevel = (Level) Enum.Parse(typeof(Level), lstLeet.SelectedValue.ToString() ?? string.Empty);
            Options.IncludePieceTitles = chkIncludeDescriptions.Checked;
            Options.IncludeReferenceDescriptions = chkIncludeReferenceTitles.Checked;
            DialogResult = DialogResult.OK;
            SaveForm();
            Close();
        }
    }

    public class PuzzleDescriptionOptions
    {
        public bool IncludePieceTitles;
        public bool IncludeReferenceDescriptions;
        public Level LeetLevel;
    }
}