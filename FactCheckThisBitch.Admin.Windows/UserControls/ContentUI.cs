using FackCheckThisBitch.Common;
using FactCheckThisBitch.Models;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace FactCheckThisBitch.Admin.Windows.UserControls
{
    public partial class ContentUi : UserControl
    {
        private IContent _content;

        public IContent Content
        {
            get
            {
                SaveForm();
                return _content;
            }
            set
            {
                _content = value;

                InitFormFields();
                InitForm();
            }
        }

        public ContentUi()
        {
            InitializeComponent();
        }

        private void SaveForm()
        {
            var properties = _content.PropertiesNotFromInterface();

            foreach (var prop in properties)
            {
                var txt = Controls.Find($"txt{prop.Name}", true).First() as TextBoxWithValidation;

                try
                {
                    var newValue = Convert.ChangeType(txt?.Text, prop.PropertyType);
                    prop.SetValue(_content, newValue);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        private void InitFormFields()
        {
            Controls.Clear();

            var index = 0;
            var textBoxPositionLeft = 100;
            var lineHeight = 25;
            var verticalPadding = 5;

            var properties = _content.PropertiesNotFromInterface();

            foreach (var prop in properties)
            {
                var lbl = new Label()
                {
                    Text = prop.Name,
                    Height = lineHeight,
                    Width = textBoxPositionLeft - 1,
                    Left = 0,
                    Top = index * (lineHeight + verticalPadding),
                    ForeColor = Color.DarkBlue
                };
                Controls.Add(lbl);

                var txt = new TextBoxWithValidation()
                {
                    Name = $"txt{prop.Name}",
                    ValidationPattern = prop.PropertyType.RegExValidationPatternForType(),
                    Width = this.Width - 250,
                    Height = lineHeight,
                    Left = textBoxPositionLeft,
                    Top = index * (lineHeight + verticalPadding),
                    Text = prop.GetValue(_content)?.ToString()
                };
                Controls.Add(txt);

                index++;
            }
        }

        private void InitForm()
        {
        }
    }
}