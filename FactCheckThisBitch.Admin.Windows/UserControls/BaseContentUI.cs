using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
        }

        private void SaveForm()
        {
            _content.Title = txtTitle.Text;
            _content.Summary = txtSummary.Text;
            _content.Source = txtSource.Text;
            _content.Url = txtUrl.Text;
            _content.References = txtReferences.Text.CommaSeparatedListToArray();
        }

        private void InitForm()
        {
            txtTitle.Text = _content.Title;
            txtSummary.Text = _content.Summary;
            txtSource.Text = _content.Source;
            txtUrl.Text = _content.Url;
            txtReferences.Text = _content.References != null ? string.Join(Environment.NewLine, _content.References) : "";
        }


    }
}
