using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Windows.Forms;
using FackCheckThisBitch.Common;

namespace FactCheckThisBitch.Admin.Windows.UserControls
{
    public partial class ImageEditor : UserControl
    {
        private List<string> _images = new List<string>();

        public List<string> Images
        {
            get => _images;
            set
            {
                _images = value;
                LoadImages();
            }
        }

        public ImageEditor()
        {
            InitializeComponent();
        }

        private void LoadImages()
        {
            var index = 0;
            var padding = 5;

            panel1.Controls.Clear();

            for(int imageIndex=0; imageIndex<Images.Count;imageIndex++)
            {
                var imageFile = Images[imageIndex];
                var imagePath = Path.Combine(Configuration.Instance().DataFolder, "media", imageFile);
                PictureBox picture = new PictureBox();
                picture.Tag = imageIndex;
                picture.Width = 100;
                picture.Height = 66;
                picture.SizeMode = PictureBoxSizeMode.StretchImage;
                picture.Image = Image.FromFile(imagePath);
                picture.Top = 0;
                picture.Left = padding + index * (picture.Width + padding);
                picture.Cursor = Cursors.Hand;
                picture.AllowDrop = true;
                picture.MouseDown += (sender, args) =>
                {
                    if (args.Button == MouseButtons.Left && args.Clicks == 1)
                    {
                        picture.DoDragDrop((sender as Control).Tag, DragDropEffects.All);
                    }
                };
                picture.DragEnter += (sender, args) =>
                {
                    args.Effect = DragDropEffects.All;
                };
                picture.DragDrop += (sender, args) =>
                {
                    var dragImageIndex = (int)args.Data.GetData(typeof(int));
                    Images.Swap<string>((int)picture.Tag, dragImageIndex);
                    LoadImages();
                };
                picture.DoubleClick += (sender, args) =>
                {
                    new Process
                    {
                        StartInfo = new ProcessStartInfo(imagePath)
                        {
                            UseShellExecute = true
                        }
                    }.Start();
                };
                new ToolTip().SetToolTip(picture, imageFile);
                panel1.Controls.Add(picture);

                var linkLabel = new LinkLabel();
                linkLabel.Text = "Delete";
                linkLabel.Click += (sender, args) =>
                {
                    _images.Remove(imageFile);
                    LoadImages();
                };
                linkLabel.Top = picture.Bottom;
                linkLabel.Left = picture.Left;
                panel1.Controls.Add(linkLabel);
                index++;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = Path.Combine(Configuration.Instance().DataFolder, "media");
            openFileDialog1.Title = "Open Image";
            openFileDialog1.DefaultExt = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif;*.tiff";
            openFileDialog1.CheckFileExists = true;
            openFileDialog1.CheckPathExists = true;
            openFileDialog1.ShowReadOnly = false;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                var imageNameWithoutPath = new FileInfo(openFileDialog1.FileName).Name;
                var destinationImage = Path.Combine(Configuration.Instance().DataFolder, "media", imageNameWithoutPath);
                if (openFileDialog1.FileName.ToLower() != destinationImage.ToLower())
                {
                    File.Copy(openFileDialog1.FileName, destinationImage);
                }

                _images.Add(imageNameWithoutPath);
                LoadImages();
            }
        }

        private void btnPaste_Click(object sender, EventArgs e)
        {
            if (Clipboard.ContainsImage())
            {
                var imageName = $"{Guid.NewGuid()}.png";
                var destinationImage = Path.Combine(Configuration.Instance().DataFolder, "media", imageName);
                Clipboard.GetImage().Save(destinationImage, ImageFormat.Png);
                _images.Add(destinationImage);
                LoadImages();
            }
        }
    }
}