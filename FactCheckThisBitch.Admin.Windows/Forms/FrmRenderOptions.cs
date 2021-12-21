using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace FactCheckThisBitch.Admin.Windows.Forms
{
    public partial class FrmRenderOptions : Form
    {
        public RenderOptions Options = new RenderOptions();

        public FrmRenderOptions()
        {
            InitializeComponent();
            InitForm();
        }

        private void InitForm()
        {
            var assetsDirectory = new DirectoryInfo(Configuration.Instance().AssetsFolder);
            var templates = assetsDirectory.GetFiles("template*pptx").Select(f => f.Name).ToList();
            lstTemplate.DataSource = templates;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Options.Template = lstTemplate.SelectedValue.ToString();
            Options.HandleWrongSpeak = chkWrongSpeak.Checked;

            DialogResult = DialogResult.OK;
            Close();
        }
    }

    public class RenderOptions
    {
        public string Template;
        public bool HandleWrongSpeak;
    }
}