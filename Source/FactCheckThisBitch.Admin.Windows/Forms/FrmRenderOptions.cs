using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using FackCheckThisBitch.Common;

namespace FactCheckThisBitch.Admin.Windows.Forms
{
    public partial class FrmRenderOptions : Form
    {
        public RenderOptions Options = new RenderOptions();

        public FrmRenderOptions()
        {
            InitializeComponent();
            InitForm();
            LoadForm();
        }

        private void InitForm()
        {
            var assetsDirectory = new DirectoryInfo(Path.Combine(Configuration.Instance().AssetsFolder,"Powerpoint"));
            var templates = assetsDirectory.GetFiles("template*pptm").Select(f => f.Name).ToList();
            lstTemplate.DataSource = templates;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void LoadForm()
        {
            if (!UserSettings.Instance().RenderOptionsTemplate.IsEmpty())
            {
                lstTemplate.SelectedItem = UserSettings.Instance().RenderOptionsTemplate;
            }

            chkBlurryAreas.Checked = UserSettings.Instance().RenderOptionsHandleBlurryAreas;
            chkWrongSpeak.Checked = UserSettings.Instance().RenderOptionsHandleWrongSpeak;
        }

        private void SaveForm()
        {
            UserSettings.Instance().RenderOptionsTemplate = lstTemplate.SelectedItem.ToString();
            UserSettings.Instance().RenderOptionsHandleBlurryAreas = chkBlurryAreas.Checked;
            UserSettings.Instance().RenderOptionsHandleWrongSpeak = chkWrongSpeak.Checked;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Options.Template = lstTemplate.SelectedValue.ToString();
            Options.HandleWrongSpeak = chkWrongSpeak.Checked;
            Options.HandleBlurryAreas = chkBlurryAreas.Checked;
            DialogResult = DialogResult.OK;

            SaveForm();
            Close();
        }
    }

    public class RenderOptions
    {
        public string Template;
        public bool HandleWrongSpeak;
        public bool HandleBlurryAreas;
    }
}