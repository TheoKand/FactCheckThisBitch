using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using FackCheckThisBitch.Common;

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

        private Dictionary<string, string> AvailableVoices = new Dictionary<string, string>()
        {
            { "ttsVoiceen-US-AriaNeural", "Grace, Female" }, { "uniform-ttsVoiceen-US-GuyNeural", "Andrew, Male" }
        };

        private void InitForm()
        {
            lstVoice.DataSource = AvailableVoices.Select(_ => _.Value).ToList();
        }

        private void LoadForm()
        {
            if (!UserSettings.Instance().NarrationOptionsVoice.IsEmpty())
            {
                lstVoice.SelectedItem =
                    AvailableVoices.First(_ => _.Key == UserSettings.Instance().NarrationOptionsVoice).Value;
            }
        }

        private void SaveForm()
        {
            UserSettings.Instance().NarrationOptionsVoice =
                AvailableVoices.First(_ => _.Value == lstVoice.SelectedItem.ToString()).Key;
        }

        private void btnCancel_Click_1(object sender, EventArgs e)
        {
            Close();
        }

        private void btnOK_Click_1(object sender, EventArgs e)
        {
            Options.Voice = AvailableVoices.First(_ => _.Value == lstVoice.SelectedItem.ToString()).Key;
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
