using FackCheckThisBitch.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Media;
using System.Threading.Tasks;
using System.Windows.Forms;
using SixLabors.ImageSharp.Drawing;
using VideoFromArticle.Models;
using WebAutomation;
using Path = System.IO.Path;

namespace VideoFromArticle.Admin.Windows.Forms
{
    public partial class FrmArticle : Form
    {
        public Action OnSave;

        private Article _article;

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
            chkRecycle.Checked = _article.RecycleImages.Value;
            chkPreview.Checked = _article.NextPreview.Value;
            chkNarrationPerImage.Checked = _article.NarrationPerImage.Value;
            txtNarration.Text = _article.Narration;
            imageEditor1.BaseFolder = _article.Folder();
            imageEditor1.BaseCaption = _article.Title;
            imageEditor1.ArticleImages = _article.Images;
        }

        private void SaveForm()
        {
            _article.Title = txtTitle.Text.ValueOrNull();
            _article.Url = txtUrl.Text.ValueOrNull();
            _article.Source = txtSource.Text.ValueOrNull();
            _article.Published = txtDatePublished.Text.ToDate();
            _article.Narration = txtNarration.Text;
            _article.RecycleImages = chkRecycle.Checked;
            _article.NextPreview = chkPreview.Checked;
            _article.NarrationPerImage = chkNarrationPerImage.Checked;
            _article.Narration = _article.Narration.SanitizeNarration();

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
            IDictionary<string, string> metaData = default;
            try
            {
                this.Cursor = Cursors.WaitCursor;
                Cursor.Current = Cursors.WaitCursor;
                Application.DoEvents();
                metaData = await onlineArticleParser.Download();

                _article.Title = metaData.TryGet("title");
                _article.Source = metaData.TryGet("site_name");
                if (DateTime.TryParse(metaData.TryGet("datePublished"), out DateTime datePublished))
                {
                    _article.Published = datePublished;
                }

                _article.Images.Clear();
                string additionalImages = metaData.TryGet("images");
                string additionalImagesWithCaptions = metaData.TryGet("imagesWithCaptions");
                string image = metaData.TryGet("image");
                if (!image.IsEmpty())
                {
                    var imageUrl = image.FilenameFromUrl();
                    if (additionalImages.IsEmpty() || !additionalImages.Contains(imageUrl))
                    {
                        if (!additionalImages.IsEmpty())
                        {
                            additionalImages += "\n";
                        }

                        additionalImages += image;
                    }
                }

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

        private void btnGenerateAudio_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (chkNarrationPerImage.Checked)
            {
                GenerateOneNarrationPerImage(); 
            }
            else
            {
                GenerateSingleNarrationForEntireArticle();
            }
        }

        private void GenerateSingleNarrationForEntireArticle()
        {
            _article.EnsureFolder();

            FrmNarrationOptions optionsForm = new FrmNarrationOptions();
            var result = optionsForm.ShowDialog();
            if (result != DialogResult.OK)
                return;

            using (var speechelo = new Speechelo())
            {
                _article.Narration = txtNarration.Text;
                _article.Narration = _article.Narration.SanitizeNarration();

                try
                {
                    speechelo.Setup();
                    speechelo.GenerateNarration(_article.Narration, optionsForm.Options.Voice,
                        _article.ArticleNarrationAudioFilePath());
                    _article.DurationInSeconds = _article.ReadNarrationDuration().TotalSeconds;

                    SaveForm();
                    OnSave?.Invoke();

                    new Process
                    {
                        StartInfo = new ProcessStartInfo(_article.ArticleNarrationAudioFilePath())
                        {
                            UseShellExecute = true
                        }
                    }.Start();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
                finally
                {
                    this.Activate();
                }
            }
        }

        private void GenerateOneNarrationPerImage()
        {
            _article.EnsureFolder();

            FrmNarrationOptions optionsForm = new FrmNarrationOptions();
            var result = optionsForm.ShowDialog();
            if (result != DialogResult.OK)
                return;

            using (var speechelo = new Speechelo())
            {
                try
                {
                    speechelo.Setup();
                    for (var articleImageIndex = 0; articleImageIndex < _article.Images.Count; articleImageIndex++)
                    {
                        var articleImage = _article.Images[articleImageIndex];

                        if (articleImage.Narration.IsNotEmpty())
                        {
                            var imageNarrationFilePath = _article.ImageNarrationAudioFilePath(articleImage);
                            speechelo.GenerateNarration(articleImage.Narration, optionsForm.Options.Voice, imageNarrationFilePath);
                            articleImage.DurationInSeconds = _article.ReadNarrationDuration(articleImage).TotalSeconds;

                            SaveForm();
                            OnSave?.Invoke();

                            new Process
                            {
                                StartInfo = new ProcessStartInfo(imageNarrationFilePath)
                                {
                                    UseShellExecute = true
                                }
                            }.Start();
                        }

                    }

                    _article.DurationInSeconds = _article.Images.Sum(_ => _.DurationInSeconds);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
                finally
                {
                    this.Activate();
                }
            }
        }

        private void btnOpenFolder_Click(object sender, EventArgs e)
        {
            var folder = _article.Folder();
            if (!Directory.Exists(folder)) return;
            new Process { StartInfo = new ProcessStartInfo(folder) { UseShellExecute = true } }.Start();
        }

        private void btnPlayNarration_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (!_article.NarrationFileExists()) return;
            _article.DurationInSeconds = _article.ReadNarrationDuration().TotalSeconds;
            new Process
            {
                StartInfo = new ProcessStartInfo(_article.ArticleNarrationAudioFilePath()) { UseShellExecute = true }
            }.Start();
        }

        private void imageEditor1_Load(object sender, EventArgs e)
        {

        }

        private void chkNarrationPerImage_CheckedChanged(object sender, EventArgs e)
        {
            if (chkNarrationPerImage.Checked && chkRecycle.Checked)
            {
                chkRecycle.Checked = false;
            }
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }
    }
}