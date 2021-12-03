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
                InitForm();
            }
        }

        public BaseContentUi()
        {
            InitializeComponent();
            InitFormFields();
        }

        public void SaveForm()
        {
            _content.Title = txtTitle.Text;
            _content.Summary = txtSummary.Text;
            _content.Source = txtSource.Text;
            _content.Url = txtUrl.Text;
            _content.References = txtReferences.Text.CommaSeparatedListToArray();
            _content.DatePublished = txtDatePublished.Text.ToDate();
        }

        private void InitFormFields()
        {
            txtDatePublished.ValidationPattern = typeof(DateTime).RegExValidationPatternForType();
        }

        private void InitForm()
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