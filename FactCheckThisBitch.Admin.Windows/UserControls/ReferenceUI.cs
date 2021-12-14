using FackCheckThisBitch.Common;
using FactCheckThisBitch.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

namespace FactCheckThisBitch.Admin.Windows.UserControls
{
    public partial class ReferenceUi : UserControl
    {
        public Action<string> OnDelete;
        public Action<string, int> OnMove;
        public Action<string> OnMoveToOtherPiece;

        private Reference _content;

        public Reference Content
        {
            get => _content;
            set
            {
                _content = value;
                LoadForm();
            }
        }

        public ReferenceUi()
        {
            InitializeComponent();
            InitForm();
        }

        public void SaveForm()
        {
            _content.Title = txtTitle.Text.ValueOrNull();
            _content.Description = txtSummary.Text.ValueOrNull();
            _content.Source = txtSource.Text.ValueOrNull();
            _content.Url = txtUrl.Text.ValueOrNull();
            _content.Type = (ReferenceType) Enum.Parse(typeof(ReferenceType), cboType.SelectedValue.ToString() ?? string.Empty);
            _content.Images = imageEditor1.Images;
            _content.Author = txtAuthor.Text;
            _content.Duration = int.Parse(txtDuration.Text);
            _content.DatePublished = txtDatePublished.Text.ToDate();
        }

        private void InitForm()
        {
            txtDuration.ValidationPattern = "^[1-9]?[0-9]$";
            txtDatePublished.ValidationPattern = typeof(DateTime).RegExValidationPatternForType();
            var pieceTypes = Enum.GetValues(typeof(ReferenceType)).Cast<ReferenceType>();
            cboType.DataSource = pieceTypes;
        }

        private void LoadForm()
        {
            cboType.SelectedItem = _content.Type;
            txtTitle.Text = _content.Title;
            txtSummary.Text = _content.Description;
            txtSource.Text = _content.Source;
            txtUrl.Text = _content.Url;
            txtDuration.Text = _content.Duration.ToString();
            imageEditor1.Images = _content.Images != null ? _content.Images.ToList() : new List<string>();
            txtDatePublished.Text = _content.DatePublished.ToSimpleStringDate();
            txtAuthor.Text = _content.Author;
        }

        #region

        private void btnUrl_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (txtUrl.Text.IsEmpty()) return;
            new Process {StartInfo = new ProcessStartInfo(txtUrl.Text) {UseShellExecute = true}}.Start();
        }

        private void btnDelete_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OnDelete?.Invoke(_content.Id);
        }

        private void btnMoveBack_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OnMove?.Invoke(_content.Id, -1);
        }

        private void btnMoveForward_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OnMove?.Invoke(_content.Id, 1);
        }

        private void btnMove_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OnMoveToOtherPiece(_content.Id);
        }

        #endregion
    }
}