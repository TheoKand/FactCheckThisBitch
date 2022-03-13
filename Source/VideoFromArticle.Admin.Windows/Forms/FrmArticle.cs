using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FackCheckThisBitch.Common;
using OpenQA.Selenium.DevTools.V85.Runtime;
using VideoFromArticle.Models;

namespace VideoFromArticle.Admin.Windows.Forms
{
    public partial class FrmArticle : Form
    {
        //public Func<string, bool> OnMoveReferenceToOtherPiece;
        public Action OnSave;

        private Article _article;

        public FrmArticle(Article article)
        {
            _article = article;
            InitializeComponent();
        }

        private async void FrmArticle_Load(object sender, EventArgs e)
        {
            InitForm();
            LoadForm();
        }

        private void SaveForm()
        {
            _article.Title = txtTitle.Text.ValueOrNull();
            _article.Url = txtUrl.Text.ValueOrNull();
        }

        private void EnsureFolders()
        {
            string articleFolder = Path.Combine(Configuration.Instance().DataFolder, _article.Id);
            if (!Directory.Exists(articleFolder))
            {
                Directory.CreateDirectory(articleFolder);
            }

            string imagesFolder = Path.Combine(Configuration.Instance().DataFolder, _article.Id, "images");
            if (!Directory.Exists(imagesFolder))
            {
                Directory.CreateDirectory(imagesFolder);
            }
        }

        private async Task FetchArticle()
        {

            var onlineArticleParser = new ArticleMetadataParser(_article.Url);
            IDictionary<string, string> metaData = default;
            try
            {
                this.Cursor = Cursors.WaitCursor;
                Cursor.Current = Cursors.WaitCursor;
                Application.DoEvents();
                metaData = await onlineArticleParser.Download();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return;
            }
            finally
            {
                Cursor.Current = Cursors.Default;
                this.Cursor = Cursors.Default;
            }

            _article.Title = metaData.TryGet("title");
            _article.Source = metaData.TryGet("site_name");
            if (DateTime.TryParse(metaData.TryGet("datePublished"), out DateTime datePublished))
            {
                _article.Published = datePublished;
            }

            _article.Images.Clear();
            string additionalImages = metaData.TryGet("images");
            string image = metaData.TryGet("image");
            if (!image.IsEmpty())
            {
                if (!additionalImages.IsEmpty())
                {
                    additionalImages += "\n";
                    additionalImages += image;
                }
            }

            EnsureFolders();
            
            if (!additionalImages.IsEmpty())
            {
                var additionalImageIndex = 1;
                foreach (var additionalImage in additionalImages.Split("\n"))
                {
                    var imageFileName = $"{_article.Id}-{_article.Title.Sanitize()}.{additionalImageIndex}.png";
                    var imagePath = Path.Combine(Configuration.Instance().DataFolder, _article.Id, "images", imageFileName);
                    
                    var fileLength = ArticleMetadataParser.SaveImage(additionalImage, imagePath);
                    if (fileLength != 0)
                    {
                        _article.Images.Add(new ArticleImage(image)
                        {
                            Filename = imageFileName,
                            Filesize = fileLength
                        });
                    }

                    additionalImageIndex++;
                    if (additionalImageIndex == 30) break;
                }
            }



            //string image = metaData.TryGet("image");
            //if (!image.IsEmpty())
            //{
            //    var imageFileName = $"{reference.Id}-{reference.Title.Sanitize()}.png";
            //    var imagePath = Path.Combine(Configuration.Instance().DataFolder, "media", imageFileName);
            //    bool save = ArticleMetadataParser.SaveImage(image, imagePath);
            //    if (save)
            //    {
            //        reference.Images.Add(imagePath);
            //    }
            //}
        }

        private void InitForm()
        {
        }

        private void LoadForm()
        {
            this.Text = _article.Title.Sanitize();

            txtTitle.Text = _article.Title;
            txtUrl.Text = _article.Url;
            imageEditor1.BaseFolder = Path.Combine(Configuration.Instance().DataFolder, _article.Id, "images");
            imageEditor1.Images = _article.Images != null ? _article.Images.Select(_=>_.Filename).ToList() : new List<string>();

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveForm();

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnUrl_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (txtUrl.Text.IsEmpty()) return;
            new Process
            {
                StartInfo = new ProcessStartInfo(txtUrl.Text)
                {
                    UseShellExecute = true
                }
            }.Start();
        }

        private async void btnDownload_Click(object sender, EventArgs e)
        {
            if (!txtUrl.Text.IsEmpty())
            {
                _article.Url = txtUrl.Text;

                await FetchArticle();

                LoadForm();
            }
        }
    }
}
