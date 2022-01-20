using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FactCheckThisBitch.Models;

namespace FactCheckThisBitch.Admin.Windows.Forms
{
    public partial class FrmSelectPiece : Form
    {

        private IReadOnlyList<string> _pieces;
        public string SelectedPieceTitle;

        public FrmSelectPiece()
        {
            InitializeComponent();
        }

        public FrmSelectPiece(IReadOnlyList<string> pieces)
        {
            InitializeComponent();
            _pieces = pieces;
            InitForm();
        }

        private void InitForm()
        {
            comboBox1.DataSource = _pieces;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            SelectedPieceTitle = comboBox1.SelectedItem.ToString();
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}