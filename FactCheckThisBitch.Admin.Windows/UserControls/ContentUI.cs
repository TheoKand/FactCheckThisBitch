using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using FackCheckThisBitch.Common;
using FactCheckThisBitch.Models;

namespace FactCheckThisBitch.Admin.Windows.UserControls
{
    public partial class ContentUI : UserControl
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

        public ContentUI()
        {
            InitializeComponent();
        }

        private void SaveForm()
        {
            var properties = _content.PropertiesNotFromInterface();

            foreach (var prop in properties)
            {
                var txt = Controls.Find($"txt{prop.Name}", true).First() as TextBoxWithValidation;

                var newValue = Convert.ChangeType(txt.Text, prop.PropertyType);
                prop.SetValue(_content,newValue);
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
