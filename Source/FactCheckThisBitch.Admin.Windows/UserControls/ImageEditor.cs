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
using VideoFromArticle.Models;

namespace FactCheckThisBitch.Admin.Windows.UserControls
{
    public partial class ImageEditor : UserControl
    {
        private List<string> _images = new List<string>();
        private List<ArticleImage> _articleImages = new List<ArticleImage>();
        public List<ImageEdit> ImageEdits;

        public string BaseFolder;
        public string BaseCaption;

        public List<string> Images
        {
            get => _images;
            set
            {
                _images = value;
                LoadForm();
            }
        }

        public List<ArticleImage> ArticleImages
        {
            get
            {
                if (_articleImages == null)
                {
                    return _images.Select(_ => new ArticleImage(null) { Filename = _ }).ToList();
                }
                else
                {
                    return _articleImages;
                }
            }
            set
            {
                _articleImages = value;
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
            var padding = 10;
            var imageLabelButtonsHeight = 20;
            var pictureHeight = this.Height - hScrollBar1.Height - imageLabelButtonsHeight - 10;
            if (chkAutosize.Checked)
            {
                var widthPerImage = (this.Width - padding * ArticleImages.Count) / ArticleImages.Count;
                pictureHeight = (int) Math.Round((widthPerImage) / 1.33, 0);
            }

            var pictureWidth = (int) Math.Round(pictureHeight * 1.33, 0);

            panel.Controls.Clear();

            for (int imageIndex = 0; imageIndex < ArticleImages.Count; imageIndex++)
            {
                var articleImage = ArticleImages[imageIndex];
                var imageFile = ArticleImages[imageIndex].Filename;
                var imagePath = Path.Combine(BaseFolder, imageFile);

                var imageFileInfo = new FileInfo(imagePath);
                PictureBox picture = new PictureBox();
                picture.Name = $"image{imageIndex}";
                picture.Tag = imageIndex;
                picture.Width = pictureWidth;
                picture.Height = pictureHeight;
                picture.SizeMode = PictureBoxSizeMode.StretchImage;
                if (File.Exists(imagePath))
                {
                    picture.Image = Image.FromFile(imagePath);
                    string tooltip =
                        $"{imageFile}\n{picture.Image?.Width}x{picture.Image?.Height} px\n{Math.Round((decimal) (imageFileInfo.Length / 1024), 0)}kb";
                    new ToolTip().SetToolTip(picture, tooltip);
                }

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
                    var toImageIndex = (int) picture.Tag;
                    var fromImageIndex = (int) args.Data.GetData(typeof(int));

                    var fromImage = ArticleImages[fromImageIndex];
                    var toImage = ArticleImages[toImageIndex];
                    ArticleImages.Remove(fromImage);
                    ArticleImages.Insert(toImageIndex, fromImage);

                    LoadForm();
                };
                picture.Click += (sender, args) =>
                {
                    //Clipboard.SetText(imagePath);
                    new Process { StartInfo = new ProcessStartInfo(imagePath) { UseShellExecute = true } }.Start();
                };

                panel.Controls.Add(picture);

                var deleteLabel = new LinkLabel();
                deleteLabel.Name = $"btnDelete{imageIndex}";
                deleteLabel.Text = "Delete";
                deleteLabel.Font = new Font("Arial", 9);

                deleteLabel.Click += (sender, args) =>
                {
                    this.ArticleImages.Remove(articleImage);
                    LoadForm();
                };
                deleteLabel.Top = picture.Bottom;
                deleteLabel.Left = picture.Left;
                deleteLabel.Width = 45;
                deleteLabel.Height = imageLabelButtonsHeight;
                deleteLabel.AutoSize = false;
                panel.Controls.Add(deleteLabel);

                var editLabel = new LinkLabel();
                editLabel.Name = $"btnEdit{imageIndex}";
                editLabel.Tag = imageFile;
                editLabel.Text = "Edit";
                editLabel.Font = new Font("Arial", 9);
                editLabel.Click += editLabel_Click;
                editLabel.Top = picture.Bottom;
                editLabel.Left = deleteLabel.Right + 2;
                editLabel.Width = 30;
                editLabel.Height = imageLabelButtonsHeight;
                editLabel.AutoSize = false;
                panel.Controls.Add(editLabel);

                var txtCaption = new TextBox();
                toolTip1.SetToolTip(txtCaption, "Caption");
                txtCaption.Name = $"txtCaption{imageIndex}";
                txtCaption.Text = articleImage.Caption;
                txtCaption.Font = new Font("Arial", 8.5f);
                txtCaption.Top = picture.Bottom;
                txtCaption.Left = editLabel.Right;
                txtCaption.Width = pictureWidth - 75;
                txtCaption.Height = imageLabelButtonsHeight;
                txtCaption.BorderStyle = BorderStyle.None;
                txtCaption.AutoSize = false;
                txtCaption.LostFocus += (sender, args) =>
                {
                    if (articleImage.Caption != txtCaption.Text)
                    {
                        articleImage.Caption = txtCaption.Text;
                    }
                };
                panel.Controls.Add(txtCaption);
                index++;
            }

            ResetScrollbar();
        }

        private void ResetScrollbar()
        {
            if (panel.Width > this.Width)
            {
                hScrollBar1.Enabled = true;
                this.hScrollBar1.Maximum = panel.Width - this.Width;
            }
            else
            {
                hScrollBar1.Enabled = false;
                this.hScrollBar1.Maximum = 0;
            }

            hScrollBar1.Value = 0;
        }

        #region events

        private void editLabel_Click(object sender, EventArgs e)
        {
            var image = (sender as Label).Tag.ToString();

            var imageEdit = this.ImageEdits?.FirstOrDefault(e => e.Image == image) ?? new ImageEdit() { Image = image };

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
            openFileDialog1.InitialDirectory = BaseFolder;
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
                    var destinationImage = Path.Combine(BaseFolder, imageNameWithoutPath);
                    if (fileNames.ToLower() != destinationImage.ToLower())
                    {
                        File.Copy(fileNames, destinationImage);
                    }

                    ArticleImages.Add(new ArticleImage(null)
                    {
                        Filename = imageNameWithoutPath, Caption = BaseCaption
                    });
                }

                LoadForm();
            }
        }

        private void btnPaste_Click(object sender, EventArgs e)
        {
            if (Clipboard.ContainsImage())
            {
                var imageName = $"{Guid.NewGuid()}.png";
                var folder = BaseFolder;
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }

                var destinationImage = Path.Combine(folder, imageName);

                Clipboard.GetImage().Save(destinationImage, ImageFormat.Png);
                ArticleImages.Add(new ArticleImage(null) { Filename = imageName, Caption = BaseCaption });
                LoadForm();
            }
        }

        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            this.panel.Left = -e.NewValue;
        }

        private void hScrollBar1_Resize(object sender, EventArgs e)
        {
            ResetScrollbar();
        }

        private void chkSmallImageSize_CheckedChanged(object sender, EventArgs e)
        {
            LoadForm();
        }

        #endregion
    }
}