using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using FactCheckThisBitch.Models;

namespace FactCheckThisBitch.Admin.Windows.UserControls
{
    public partial class BaseContentUI : UserControl
    {
        private BaseContent _content;
        public BaseContent Content
        {
            get => _content;
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

        private void InitForm()
        {
            txtTitle.Text = _content.Title;
            txtSummary.Text = _content.Summary;
            txtSource.Text = _content.Source;
            txtUrl.Text = _content.Url?.AbsoluteUri;
            txtReferences.Text = _content.References !=null ? string.Join(Environment.NewLine, _content.References):"";
        }


    }
}
