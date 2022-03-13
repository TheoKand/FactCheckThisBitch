﻿using FackCheckThisBitch.Common;
using FactCheckThisBitch.Admin.Windows.Forms;
using FactCheckThisBitch.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace FactCheckThisBitch.Admin.Windows.UserControls
{
    public partial class ImageEditor : UserControl
    {
        private List<string> _images = new List<string>();
        public List<ImageEdit> ImageEdits;

        public string BaseFolder;

        public List<string> Images
        {
            get => _images;
            set
            {
                _images = value;
                LoadForm();
            }
        }

        public ImageEditor()
        {
            InitializeComponent();
            toolTip1.SetToolTip(this, "Right click to open image; left button to drag & drop reposition");
        }

        private void LoadForm()
        {
            var index = 0;
            var padding = 30;

            panel1.Controls.Clear();

            for (int imageIndex = 0; imageIndex < Images.Count; imageIndex++)
            {
                var imageFile = Images[imageIndex];
                var imagePath = Path.Combine(BaseFolder, imageFile);
                PictureBox picture = new PictureBox();
                picture.Tag = imageIndex;
                picture.Width = 100;
                picture.Height = 66;
                picture.SizeMode = PictureBoxSizeMode.StretchImage;
                picture.Image = Image.FromFile(imagePath);
                picture.Top = 0;
                picture.Left = index * (picture.Width + padding);
                picture.Cursor = Cursors.Hand;
                picture.AllowDrop = true;
                picture.MouseDown += (sender, args) =>
                {
                    if (args.Button == MouseButtons.Left && args.Clicks == 1)
                    {
                        picture.DoDragDrop((sender as Control).Tag, DragDropEffects.All);
                    }
                };

                picture.DragEnter += (sender, args) => { args.Effect = DragDropEffects.All; };
                picture.DragDrop += (sender, args) =>
                {
                    var dragImageIndex = (int) args.Data.GetData(typeof(int));
                    Images.Swap<string>((int) picture.Tag, dragImageIndex);
                    LoadForm();
                };
                picture.Click += (sender, args) =>
                {
                    Clipboard.SetText(imagePath);
                    new Process {StartInfo = new ProcessStartInfo(imagePath) {UseShellExecute = true}}.Start();
                };
                new ToolTip().SetToolTip(picture, imageFile);
                panel1.Controls.Add(picture);

                var deleteLabel = new LinkLabel();
                deleteLabel.Text = "Delete";

                deleteLabel.Click += (sender, args) =>
                {
                    _images.Remove(imageFile);
                    LoadForm();
                };
                deleteLabel.Top = picture.Bottom;
                deleteLabel.Left = picture.Left;
                deleteLabel.Width = 60;
                deleteLabel.Height = 20;
                deleteLabel.AutoSize = false;
                panel1.Controls.Add(deleteLabel);

                var editLabel = new LinkLabel();
                editLabel.Tag = imageFile;
                editLabel.Text = "Edit";
                editLabel.Click += editLabel_Click;
                editLabel.Top = picture.Bottom;
                editLabel.Left = deleteLabel.Right + 10;
                editLabel.Width = 60;
                editLabel.Height = 20;
                editLabel.AutoSize = false;
                panel1.Controls.Add(editLabel);

                index++;
            }
        }

        #region events

        private void editLabel_Click(object sender, EventArgs e)
        {
            var image = (sender as Label).Tag.ToString();

            var imageEdit = this.ImageEdits.FirstOrDefault(e => e.Image == image) ?? new ImageEdit() {Image = image};

            FrmImageEdit editForm = new FrmImageEdit(imageEdit);
            var result = editForm.ShowDialog();
            if (result != DialogResult.OK) return;

            var existingImageEdit = ImageEdits.FirstOrDefault(e => e.Image == imageEdit.Image);
            if (existingImageEdit == null)
            {
                ImageEdits.Add(imageEdit);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = Path.Combine(Configuration.Instance().DataFolder, "media");
            openFileDialog1.Title = "Open Image";
            openFileDialog1.DefaultExt = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif;*.tiff";
            openFileDialog1.CheckFileExists = true;
            openFileDialog1.CheckPathExists = true;
            openFileDialog1.Multiselect = true;
            openFileDialog1.ShowReadOnly = false;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                foreach (string fileNames in openFileDialog1.FileNames)
                {
                    var imageNameWithoutPath = new FileInfo(fileNames).Name;
                    var destinationImage = Path.Combine(Configuration.Instance().DataFolder, "media", imageNameWithoutPath);
                    if (fileNames.ToLower() != destinationImage.ToLower())
                    {
                        File.Copy(fileNames, destinationImage);
                    }

                    _images.Add(imageNameWithoutPath);
                }

                LoadForm();
            }
        }

        private void btnPaste_Click(object sender, EventArgs e)
        {
            if (Clipboard.ContainsImage())
            {
                var imageName = $"{Guid.NewGuid()}.png";
                var destinationImage = Path.Combine(Configuration.Instance().DataFolder, "media", imageName);
                Clipboard.GetImage().Save(destinationImage, ImageFormat.Png);
                _images.Add(imageName);
                LoadForm();
            }
        }

        #endregion
    }
}