using FackCheckThisBitch.Common;
using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace VideoFromArticle.Admin.Windows.Forms
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
            var templatesFolder = new DirectoryInfo(Configuration.Instance().VideoFromArticleTempatesFolder);

            //available templates are subfolders of templates folder that don't start with _
            var templates = templatesFolder.GetDirectories().Where(f => !f.Name.StartsWith("_")).Select(_=>_.Name).ToList();
            lstTemplate.DataSource = templates;
        }
        private void LoadForm()
        {
            if (!UserSettings.Instance().RenderOptionsTemplate.IsEmpty())
            {
                lstTemplate.SelectedItem = UserSettings.Instance().RenderOptionsTemplate;
            }

            var durationTimeSpan = UserSettings.Instance()
                .RenderOptionsIntroDuration.ToString()
                .ParseTimeSpan();

            txtIntroDuration.Text = durationTimeSpan.ToString("mm\\:ss");

        }

        private void SaveForm()
        {
            UserSettings.Instance().RenderOptionsTemplate = Options.Template;
            UserSettings.Instance().RenderOptionsIntroDuration = Options.IntroDurationSeconds;
            UserSettings.Instance().Save();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Options.Template = lstTemplate.SelectedValue.ToString();
            Options.IntroDurationSeconds = (int)txtIntroDuration.Text.ParseTimeSpan().TotalSeconds;
            DialogResult = DialogResult.OK;

            SaveForm();
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }

    public class RenderOptions
    {
        public string Template;
        public int IntroDurationSeconds;
    }
}
