using FackCheckThisBitch.Common;
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
using Article = FactCheckThisBitch.Models.Article;

namespace FactCheckThisBitch.Admin.Windows.UserControls
{
    public partial class ArticleImageEditor : UserControl
    {
        private List<ArticleImage> _articleImages = new List<ArticleImage>();
        public List<ImageEdit> ImageEdits;

        public string BaseFolder;
        public string BaseCaption;

        public List<ArticleImage> ArticleImages
        {
            get => _articleImages;
            set
            {
                _articleImages = value;
                LoadForm();
            }
        }

        public ArticleImageEditor()
        {
            InitializeComponent();
            toolTip1.SetToolTip(this, "Right click to open image; left button to drag & drop reposition");
        }

        private void LoadForm()
        {
            var index = 0;
            var padding = 10;
            var imageLabelButtonsHeight = 20;

            var pictureWidth = 400;
            var pictureHeight = (int)Math.Round(pictureWidth / 1.33,0);

            if (chkAutosize.Checked)
            {
                pictureHeight = (this.Height / ArticleImages.Count) - (padding + imageLabelButtonsHeight) ;
                pictureWidth = (int)Math.Round(pictureHeight * 1.33,0);
            }

            panel.Height = ArticleImages.Count * (pictureHeight + imageLabelButtonsHeight + padding)+ 50;
            panel.Controls.Clear();

            for (int imageIndex = 0; imageIndex < ArticleImages.Count; imageIndex++)
            {
                var articleImage = ArticleImages[imageIndex];
                var imageFile = ArticleImages[imageIndex].Filename;
                var imagePath = Path.Combine(BaseFolder, imageFile);
                var pictureTop = index * (pictureHeight + padding + imageLabelButtonsHeight);

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
                        $"{imageIndex}\t{imageFile}\n{picture.Image?.Width}x{picture.Image?.Height} px\n{Math.Round((decimal) (imageFileInfo.Length / 1024), 0)}kb";
                    new ToolTip().SetToolTip(picture, tooltip);
                }

                picture.Top = pictureTop;
                picture.Left = 0;
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

                var chkTypewriter = new CheckBox();
                chkTypewriter.Name = $"chkTypewriter{imageIndex}";
                chkTypewriter.Text = "Typewriter";
                chkTypewriter.Left = txtCaption.Right;
                chkTypewriter.Top = picture.Bottom;
                chkTypewriter.Checked = articleImage.TypewriterAnimation;
                chkTypewriter.CheckedChanged += (sender, args) =>
                {
                    articleImage.TypewriterAnimation = (sender as CheckBox).Checked;
                };
                panel.Controls.Add(chkTypewriter);
                

                var txtNarration = new TextBox();
                txtNarration.Name = $"txtNarration{imageIndex}";
                txtNarration.Text = articleImage.Narration;
                txtNarration.Top = pictureTop;
                txtNarration.Left = pictureWidth;
                txtNarration.Width = this.Width - pictureWidth-padding;
                txtNarration.Height = picture.Height;
                txtNarration.Multiline = true;
                txtNarration.ScrollBars = ScrollBars.Both;
                toolTip1.SetToolTip(txtNarration, $"Duration: {articleImage.DurationInSeconds} sec");
                txtNarration.LostFocus += (sender, args) =>
                {
                    if (articleImage.Narration != txtNarration.Text)
                    {
                        articleImage.Narration = txtNarration.Text;
                        articleImage.Narration = articleImage.Narration.SanitizeNarration();

                        //if it's just a pause
                        if (articleImage.Narration.StartsWith("<duration>"))
                        {
                            var duration = articleImage.Narration.Replace("<duration>", "").Replace("</duration>", "");
                            articleImage.DurationInSeconds = Double.Parse(duration);
                        }
                    }
                };
                panel.Controls.Add(txtNarration);

                index++;
            }

            ResetScrollbar();
        }

        private void ResetScrollbar()
        {
            if (panel.Height > this.Height - panel.Top)
            {
                vScrollBar1.Enabled = true;
                vScrollBar1.Maximum = (panel.Height - (this.Height - panel.Top)) /4;
            }
            else
            {
                vScrollBar1.Enabled = false;
                vScrollBar1.Maximum = 0;
            }

            vScrollBar1.Value = 0;
            panel.Top = 0;
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

        #endregion

        private void vScrollBar1_Resize(object sender, EventArgs e)
        {
            ResetScrollbar();
        }

        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            panel.Top = -e.NewValue*4;
        }

        private void chkAutosize_CheckedChanged(object sender, EventArgs e)
        {
            LoadForm();
        }
    }
}