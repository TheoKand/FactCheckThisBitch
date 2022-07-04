using FackCheckThisBitch.Common;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using VideoFromArticle.Models;
using Path = System.IO.Path;

namespace VideoFromArticle.Admin.Windows.Forms
{
    public partial class FrmArticle : Form
    {
        public Action OnSave;

        private readonly Article _article;

        public FrmArticle(Article article)
        {
            _article = article;
            InitializeComponent();
        }

        private void FrmArticle_Load(object sender, EventArgs e)
        {
            InitForm();
            LoadForm();
        }

        private void InitForm()
        {
        }

        private void LoadForm()
        {
            UpdateFormTitle();

            txtTitle.Text = _article.Title;
            txtAudioFile.Text = _article.Title.Sanitize().Limit(30);
            txtUrl.Text = _article.Url;
            txtSource.Text = _article.Source;
            txtDatePublished.Text = _article.Published.ToSimpleStringDate();
            imageEditor1.OnDeleteAudio = image =>
            {
                _article.DeleteNarrationDuration(image);
            };
            imageEditor1.BaseFolder = _article.Folder();
            imageEditor1.BaseCaption = "";
            imageEditor1.ArticleImages = _article.Images;
        }

        private void SaveForm()
        {
            _article.Title = txtTitle.Text.ValueOrNull();
            _article.Url = txtUrl.Text.ValueOrNull();
            _article.Source = txtSource.Text.ValueOrNull();
            _article.Published = txtDatePublished.Text.ToDate();

            //TODO: make sure SanitizeNarration is applied to new narrations after they are typed

            UpdateFormTitle();
        }

        private void UpdateFormTitle()
        {
            Text = $"{_article.Id} {_article.Diagnostics()}";
        }

        private string GetImageFromImageWithCaption(string imageWithCaptionSeparatedByTab)
        {
            var parts = imageWithCaptionSeparatedByTab.Split("\t");
            if (parts.Length == 0) return imageWithCaptionSeparatedByTab;
            return parts[0];
        }

        private string GetCaptionFromImageWithCaption(string imageWithCaptionSeparatedByTab)
        {
            var parts = imageWithCaptionSeparatedByTab.Split("\t");
            if (parts.Length == 0) return imageWithCaptionSeparatedByTab;
            return parts[1];
        }

        private async Task DownloadAndSaveMetadataAndImages()
        {
            var onlineArticleParser = new ArticleMetadataParser(_article.Url);
            try
            {
                this.Cursor = Cursors.WaitCursor;
                Cursor.Current = Cursors.WaitCursor;
                Application.DoEvents();
                var metaData = await onlineArticleParser.Download();

                _article.Title = metaData.TryGet("title");
                _article.Source = metaData.TryGet("site_name");
                if (DateTime.TryParse(metaData.TryGet("datePublished"), out var datePublished))
                {
                    _article.Published = datePublished;
                }

                _article.Images.Clear();
                var additionalImagesWithCaptions = metaData.TryGet("imagesWithCaptions");
                var image = metaData.TryGet("image");
                //var additionalImages = metaData.TryGet("images");
                //if (!image.IsEmpty())
                //{
                //    var imageUrl = image.FilenameFromUrl();
                //    if (additionalImages.IsEmpty() || !additionalImages.Contains(imageUrl))
                //    {
                //        if (!additionalImages.IsEmpty())
                //        {
                //            additionalImages += "\n";
                //        }
                //    }
                //}

                _article.EnsureFolder();

                if (!additionalImagesWithCaptions.IsEmpty())
                {
                    foreach (var additionalImage in additionalImagesWithCaptions.Split("\n"))
                    {
                        var imageUrl = GetImageFromImageWithCaption(additionalImage);
                        var imageCaption = GetCaptionFromImageWithCaption(additionalImage);

                        var imageFileName = imageUrl.FilenameFromUrl();
                        var imagePath = Path.Combine(_article.Folder(), imageFileName);

                        var (fileInfo, width, height) =
                            ArticleMetadataParser.SaveImage(imageUrl.UrlWithoutQuerystring(), imagePath);
                        if (fileInfo?.Length > StaticSettings.MinimumArticleImageSize &&
                            width >= StaticSettings.MinimumArticleImageWidth &&
                            height >= StaticSettings.MinimumArticleImageHeight)
                        {
                            _article.Images.Add(new ArticleImage(image)
                            {
                                Filename = imageFileName,
                                Filesize = fileInfo.Length,
                                Caption = imageCaption.IsEmpty() ? _article.Title : imageCaption,
                                Narration = imageCaption.IsEmpty() ? "" : imageCaption,
                            });
                        }
                        else
                        {
                            if (File.Exists(imagePath))
                            {
                                File.Delete(imagePath);
                            }
                        }
                    }
                }

                MessageBox.Show("Finished");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                Cursor.Current = Cursors.Default;
                this.Cursor = Cursors.Default;
                Application.DoEvents();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveForm();
            OnSave?.Invoke();
            UpdateFormTitle();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnUrl_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (txtUrl.Text.IsEmpty()) return;
            new Process { StartInfo = new ProcessStartInfo(txtUrl.Text) { UseShellExecute = true } }.Start();
        }

        private void btnOpenAllImages_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            foreach (var image in _article.Images.Select(_ => _.Filename))
            {
                var imagePath = Path.Combine(_article.Folder(), image);
                new Process { StartInfo = new ProcessStartInfo(imagePath) { UseShellExecute = true } }.Start();
            }
        }

        private async void btnDownload_Click(object sender, EventArgs e)
        {
            if (txtUrl.Text.IsEmpty()) return;

            _article.Url = txtUrl.Text;
            await DownloadAndSaveMetadataAndImages();
            OnSave?.Invoke();

            LoadForm();
        }

        private void btnOpenFolder_Click(object sender, EventArgs e)
        {
            var folder = _article.Folder();
            if (!Directory.Exists(folder)) return;
            new Process { StartInfo = new ProcessStartInfo(folder) { UseShellExecute = true } }.Start();
        }

    }
}