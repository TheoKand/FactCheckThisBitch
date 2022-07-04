using FackCheckThisBitch.Common;
using System;
using System.Linq;
using System.Windows.Forms;

namespace VideoFromArticle.Admin.Windows.Forms
{
    public partial class FrmNarrationOptions : Form
    {
        public NarrationOptions Options = new NarrationOptions();

        public FrmNarrationOptions()
        {
            InitializeComponent();
            InitForm();
            LoadForm();
        }

        private void InitForm()
        {
            lstVoice.DataSource = StaticSettings.AvailableVoices.Select(_ => _.Value).ToList();
        }

        private void LoadForm()
        {
            if (!UserSettings.Instance().NarrationOptionsVoice.IsEmpty())
            {
                lstVoice.SelectedItem =
                    StaticSettings.AvailableVoices.First(_ => _.Key == UserSettings.Instance().NarrationOptionsVoice).Value;
            }
        }

        private void SaveForm()
        {
            UserSettings.Instance().NarrationOptionsVoice =
                StaticSettings.AvailableVoices.First(_ => _.Value == lstVoice.SelectedItem.ToString()).Key;
            UserSettings.Instance().Save();
        }

        private void btnCancel_Click_1(object sender, EventArgs e)
        {
            Close();
        }

        private void btnOK_Click_1(object sender, EventArgs e)
        {
            Options.Voice = StaticSettings.AvailableVoices.First(_ => _.Value == lstVoice.SelectedItem.ToString()).Key;
            DialogResult = DialogResult.OK;
            SaveForm();
            Close();
        }
    }

    public class NarrationOptions
    {
        public string Voice;
    }
}
