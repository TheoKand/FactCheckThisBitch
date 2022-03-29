using System.Windows.Forms;
using FackCheckThisBitch.Common;

namespace VideoFromArticle.Admin.Windows.Forms
{
    public static class Prompt
    {
        public static string ShowDialog(string text, string caption,string defaultValue="")
        {
            Form prompt = new Form()
            {
                Width = 500,
                Height = 250,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = caption,
                StartPosition = FormStartPosition.CenterParent
            };
            Label textLabel = new Label() { Left = 20, Top = 20, Text = text };
            TextBox textBox = new TextBox() { Left = 20, Top = 50, Width = 420, Text= defaultValue };
            Button confirmation = new Button() { Text = "Ok", Left = 350, Width = 100, Top = 100, Height = 45, DialogResult = DialogResult.OK };
            confirmation.Click += (sender, e) => { prompt.Close(); };
            prompt.Controls.Add(textBox);
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(textLabel);
            prompt.AcceptButton = confirmation;
            if (Clipboard.ContainsText())
            {
                string url = Clipboard.GetText();
                if (url.IsValidUrl())
                {
                    textBox.Text = url;
                }
            }
            textBox.Focus();
            return prompt.ShowDialog() == DialogResult.OK ? textBox.Text : "";
        }
    }
}