using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using FackCheckThisBitch.Common;
using FactCheckThisBitch.Models;

namespace FactCheckThisBitch.Admin.Windows.UserControls
{
    public partial class BaseContentUI : UserControl
    {
        private BaseContent _content;
        public BaseContent Content
        {
            get
            {
                SaveForm();
                return _content;
            }
            set
            {
                _content = value;
                InitForm();
            }
        }

        public BaseContentUI()
        {
            InitializeComponent();
            InitFormFields();

        }

        private void SaveForm()
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
            txtDatePublished.ValidationPattern = "^(?:(?:31(\\/|-|\\.)(?:0?[13578]|1[02]))\\1|(?:(?:29|30)(\\/|-|\\.)(?:0?[13-9]|1[0-2])\\2))(?:(?:1[6-9]|[2-9]\\d)?\\d{2})$|^(?:29(\\/|-|\\.)0?2\\3(?:(?:(?:1[6-9]|[2-9]\\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00))))$|^(?:0?[1-9]|1\\d|2[0-8])(\\/|-|\\.)(?:(?:0?[1-9])|(?:1[0-2]))\\4(?:(?:1[6-9]|[2-9]\\d)?\\d{2})$";
        }

        private void InitForm()
        {
            txtTitle.Text = _content.Title;
            txtSummary.Text = _content.Summary;
            txtSource.Text = _content.Source;
            txtUrl.Text = _content.Url;
            txtReferences.Text = _content.References != null ? string.Join(Environment.NewLine, _content.References) : "";
            txtDatePublished.Text = _content.DatePublished.ToSimpleStringDate();
        }

        #region
        private void btnUrl_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUrl.Text)) return;
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
