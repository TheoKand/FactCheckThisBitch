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
        private BaseContentUI _baseContentUi;
        private ContentUI _contentUI;
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
            imageEditor1.Images = _piece.Images.ToList();

            panelContent.Controls.Clear();
            LoadBaseContentUI();
            LoadContentUI();

        }

        private void SaveForm()
        {
            _piece.Title = txtTitle.Text;
            _piece.Thesis = txtThesis.Text;
            _piece.Keywords = txtKeywords.Text.CommaSeparatedListToArray();
            _piece.Images = imageEditor1.Images.ToArray();
            _piece.Images = imageEditor1.Images.ToArray();
            _piece.Type = (PieceType) Enum.Parse(typeof(PieceType), cboType.SelectedValue.ToString());

            var baseContent = _baseContentUi.Content;
            var content = _contentUI.Content;

            _piece.Content = _baseContentUi.Content;

        }

        private void LoadBaseContentUI()
        {
            lblContent.Text = _piece.Type.ToString();

            _baseContentUi = new BaseContentUI();
            _baseContentUi.Content = _piece.Content;
            _baseContentUi.Left = 4;
            _baseContentUi.Top = 12;
            _baseContentUi.Width = panelContent.Width - 8;

            panelContent.Controls.Add(_baseContentUi);

        }

        private void LoadContentUI()
        {
            _contentUI = new ContentUI();
            _contentUI.Content = _piece.Content;
            _contentUI.Left = 4;
            _contentUI.Top = _baseContentUi.Bottom;
            _contentUI.Width = _baseContentUi.Width - 8;
            _contentUI.Height = _piece.Content.PropertiesNotFromInterface().Count() * 30;

            panelContent.Controls.Add(_contentUI);
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

            panelContent.Controls.Clear();
            LoadBaseContentUI();
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
