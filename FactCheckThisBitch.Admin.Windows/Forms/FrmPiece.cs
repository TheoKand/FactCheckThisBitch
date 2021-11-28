using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FackCheckThisBitch.Common;
using FactCheckThisBitch.Admin.Windows.UserControls;
using FactCheckThisBitch.Models;

namespace FactCheckThisBitch.Admin.Windows.Forms
{
    public partial class FrmPiece : Form
    {
        private Piece _piece;
        private BaseContentUI _contentUi;
        private bool _loading = false;

        public FrmPiece(Piece piece)
        {
            _piece = piece;
            InitializeComponent();
        }

        private void FrmPieceEdit_Load(object sender, EventArgs e)
        {
            _loading = true;
            InitFormFields();
            InitForm();
            _loading = false;
        }

        private void InitFormFields()
        {
            var pieceTypes = Enum.GetValues(typeof(PieceType)).Cast<PieceType>();
            cboType.DataSource = pieceTypes;
        }

        private void InitForm()
        {
            this.Text = _piece.Title.ToSanitizedString();

            cboType.SelectedItem = _piece.Type;
            txtTitle.Text = _piece.Title;
            txtThesis.Text = _piece.Thesis;
            txtKeywords.Text = string.Join(", ", _piece.Keywords);
            txtImages.Text = string.Join(", ", _piece.Images);

            LoadContentUI();

        }

        private void SaveForm()
        {
            _piece.Title = txtTitle.Text;
            _piece.Thesis = txtThesis.Text;
            _piece.Keywords = txtKeywords.Text.Split(",").Select(x=>x.Trim()).ToArray();
            _piece.Images = txtImages.Text.Split(",").Select(x=>x.Trim()).ToArray();
            _piece.Type = (PieceType) Enum.Parse(typeof(PieceType), cboType.SelectedValue.ToString());
        }

        private void LoadContentUI()
        {
            lblContent.Text = _piece.Type.ToString();

            var contentUi = new BaseContentUI();
            contentUi.Content = _piece.Content;
            contentUi.Left = 4;
            contentUi.Top = 12;
            contentUi.Width = groupBoxContent.Width - 8;
            contentUi.Height = groupBoxContent.Height - 24;
            groupBoxContent.Controls.Clear();
            groupBoxContent.Controls.Add(contentUi);
        }

        #region events
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cboType_SelectedValueChanged(object sender, EventArgs e)
        {
            if (_loading) return;
            var newType = (PieceType) Enum.Parse(typeof(PieceType), cboType.SelectedValue.ToString());
            _piece.Type = newType;
            LoadContentUI();

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveForm();

            DialogResult = DialogResult.OK;
            Close();
        }
        #endregion
    }
}
