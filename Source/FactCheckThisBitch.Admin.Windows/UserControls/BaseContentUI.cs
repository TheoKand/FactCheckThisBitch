using FackCheckThisBitch.Common;
using FactCheckThisBitch.Models;
using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace FactCheckThisBitch.Admin.Windows.UserControls
{
    public partial class BaseContentUi : UserControl
    {
        private IContent _content;

        public IContent Content
        {
            get => _content;
            set
            {
                _content = value;
                LoadForm();
            }
        }

        public BaseContentUi()
        {
            InitializeComponent();
            InitForm();
        }

        public void SaveForm()
        {
            _content.Title = txtTitle.Text.ValueOrNull();
            _content.Summary = txtSummary.Text.ValueOrNull();
            _content.Source = txtSource.Text.ValueOrNull();
            _content.Url = txtUrl.Text.ValueOrNull();
            _content.References = txtReferences.Text.CommaSeparatedListToArray();
            _content.DatePublished = txtDatePublished.Text.ToDate();
        }

        private void InitForm()
        {
            txtDatePublished.ValidationPattern = typeof(DateTime).RegExValidationPatternForType();
        }

        private void LoadForm()
        {
            txtTitle.Text = _content.Title;
            txtSummary.Text = _content.Summary;
            txtSource.Text = _content.Source;
            txtUrl.Text = _content.Url;
            txtReferences.Text =
                _content.References != null ? string.Join(Environment.NewLine, _content.References) : "";
            txtDatePublished.Text = _content.DatePublished.ToSimpleStringDate();
        }

        #region

        private void btnUrl_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (txtUrl.Text.IsEmpty()) return;
            new Process
            {
                StartInfo = new ProcessStartInfo(txtUrl.Text)
                {
                    UseShellExecute = true
                }
            }.Start();
        }

        #endregion
    }
}