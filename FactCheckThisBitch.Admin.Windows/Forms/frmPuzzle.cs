using FactCheckThisBitch.Models;
using System;
using System.Diagnostics.Eventing.Reader;
using System.Windows.Forms;
using FackCheckThisBitch.Common;

namespace FactCheckThisBitch.Admin.Windows.Forms
{
    public partial class FrmPuzzle : Form
    {
        private Puzzle _puzzle;

        private bool EditMode => _puzzle != null;

        public FrmPuzzle()
        {
            InitializeComponent();
        }

        public FrmPuzzle(Puzzle puzzle)
        {
            _puzzle = puzzle;
            InitializeComponent();
        }

        private void frmPuzzleMetadata_Load(object sender, EventArgs e)
        {
            SetTitle();
            InitForm();
        }

        private void SetTitle()
        {
            this.Text = EditMode ? $"Edit Puzzle: {_puzzle.Title.ToSanitizedString()}" : "Create New Puzzle";
        }

        private void InitForm()
        {
            if (_puzzle != null)
            {
                txtTitle.Text = _puzzle.Title;
                txtThesis.Text = _puzzle.Thesis;
                cboWidth.SelectedIndex = _puzzle.Width - 3;
                cboHeight.SelectedIndex = _puzzle.Height - 3;
            }
            else
            {
                txtTitle.Text = "";
                txtThesis.Text = "";
                cboWidth.SelectedIndex = 0;
                cboHeight.SelectedIndex = 0;
            }
        }

        #region Events
        private void btnOk_Click(object sender, EventArgs e)
        {

        }
        #endregion

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
