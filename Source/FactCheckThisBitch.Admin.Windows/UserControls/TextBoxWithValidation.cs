using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace FactCheckThisBitch.Admin.Windows.UserControls
{
    public partial class TextBoxWithValidation : UserControl
    {
        public string ValidationPattern;
        public Action<string> TextChanged;

        private string _validatedText;
        public string Text
        {
            get => textBox1.Text;
            set
            {
                textBox1.Text = value;
            }
        }

        public TextBoxWithValidation()
        {
            InitializeComponent();

        }



        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (ValidationPattern == null) return;
            var result = Regex.Match(textBox1.Text, ValidationPattern);
            if (!result.Success)
            {
                textBox1.ForeColor = Color.Red;
            }
            else
            {
                textBox1.ForeColor = Color.Black;
                _validatedText = textBox1.Text;
                TextChanged?.Invoke(_validatedText);
            }
        }

        private void TextBoxWithValidation_VisibleChanged(object sender, EventArgs e)
        {
            if (!Visible) return;
            textBox1.Left = 0;
            textBox1.Top = 0;
            textBox1.Width = this.Width-1;
            textBox1.Height = this.Height-1;
        }
    }
}
