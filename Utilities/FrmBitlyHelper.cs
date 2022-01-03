using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PowerPointPOC
{
    public partial class FrmBitlyHelper : Form
    {
        public FrmBitlyHelper()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //get lines from export

            var description = txtDescription.Text;
            var lines = txtBitlyExport.Text.Split(Environment.NewLine);
            foreach (var line in lines.Where(l=>l.Trim()!=""))
            {   
                var longLink = line.Split("\t")[2];
                var shortLink = line.Split("\t")[0];

                description = description.Replace(longLink, shortLink);
            }

            txtDescription.Text = description;
        }
    }
}
