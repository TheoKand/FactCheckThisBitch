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
using VideoFromArticle.Models;
using WebAutomation;

namespace VideoFromArticle.Admin.Windows.Forms
{
    public partial class FrmArticle : Form
    {
        public Action OnSave;

        private SlideshowArticle _slideshowArticle;

        public FrmArticle(SlideshowArticle slideshowArticle)
        {
            _slideshowArticle = slideshowArticle;
            InitializeComponent();
        }

        private void FrmArticle_Load(object sender, EventArgs e)
        {
            InitForm();
            LoadForm();
        }

        private void SaveForm()
        {
            _slideshowArticle.Title = txtTitle.Text.ValueOrNull();
            _slideshowArticle.Url = txtUrl.Text.ValueOrNull();
            _slideshowArticle.Source = txtSource.Text.ValueOrNull();
            _slideshowArticle.Published = txtDatePublished.Text.ToDate();
            _slideshowArticle.Narration = txtNarration.Text;
            _slideshowArticle.SanitizeNarration();

            _slideshowArticle.Images = imageEditor1.Images.Select(_=>new ArticleImage(null)
            {
                Filename = _
            }).ToList();

            Text = $"{_slideshowArticle.Id} {_slideshowArticle.Diagnostics()}";
        }

        private void EnsureFolders()
        {
            if (!Directory.Exists(_slideshowArticle.Folder()))
            {
                Directory.CreateDirectory(_slideshowArticle.Folder());
            }

            if (!Directory.Exists(_slideshowArticle.ImagesFolder()))
            {
                Directory.CreateDirectory(_slideshowArticle.ImagesFolder());
            }
        }

        private async Task DownloadAndSaveMetadataAndImages()
        {

            var onlineArticleParser = new ArticleMetadataParser(_slideshowArticle.Url);
            IDictionary<string, string> metaData = default;
            try
            {
                this.Cursor = Cursors.WaitCursor;
                Cursor.Current = Cursors.WaitCursor;
                Application.DoEvents();
                metaData = await onlineArticleParser.Download();


                _slideshowArticle.Title = metaData.TryGet("title");
                _slideshowArticle.Source = metaData.TryGet("site_name");
                if (DateTime.TryParse(metaData.TryGet("datePublished"), out DateTime datePublished))
                {
                    _slideshowArticle.Published = datePublished;
                }

                _slideshowArticle.Images.Clear();
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

                    foreach (var additionalImage in additionalImages.Split("\n"))
                    {
                        var additionalImageIndex = _slideshowArticle.Images.Count + 1;
                        var imageFileName = $"{_slideshowArticle.Id}-{_slideshowArticle.Title.Sanitize()}.{additionalImageIndex}.png";
                        var imagePath = Path.Combine(_slideshowArticle.ImagesFolder(), imageFileName);

                        var (fileInfo, width, height) = ArticleMetadataParser.SaveImage(additionalImage, imagePath);
                        if (fileInfo.Length> StaticSettings.MinimumArticleImageSize && width>= StaticSettings.MinimumArticleImageWidth && height >= StaticSettings.MinimumArticleImageHeight)
                        {
                            _slideshowArticle.Images.Add(new ArticleImage(image)
                            {
                                Filename = imageFileName,
                                Filesize = fileInfo.Length
                            });
                        }
                        else
                        {
                            File.Delete(imagePath);
                        }

                        if (_slideshowArticle.Images.Count >= StaticSettings.MaximumArticleImageCount) break;
                    }
                }

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
                SystemSounds.Beep.Play();
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
            Text = $"{_slideshowArticle.Id} {_slideshowArticle.Diagnostics()}";

            txtTitle.Text = _slideshowArticle.Title;
            txtUrl.Text = _slideshowArticle.Url;
            txtSource.Text = _slideshowArticle.Source;
            txtDatePublished.Text = _slideshowArticle.Published.ToSimpleStringDate();
            txtNarration.Text = _slideshowArticle.Narration;
            imageEditor1.BaseFolder = _slideshowArticle.ImagesFolder();
            imageEditor1.Images = _slideshowArticle.Images != null ? _slideshowArticle.Images.Select(_ => _.Filename).ToList() : new List<string>();

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveForm();
            OnSave?.Invoke();

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



        private void btnOpenAllImages_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            foreach (var image in _slideshowArticle.Images.Select(_=>_.Filename))
            {
                var imagePath = Path.Combine(_slideshowArticle.ImagesFolder(), image);
                new Process { StartInfo = new ProcessStartInfo(imagePath) { UseShellExecute = true } }.Start();
            }
        }

        private async void btnDownload_Click(object sender, EventArgs e)
        {
            if (txtUrl.Text.IsEmpty()) return;

            _slideshowArticle.Url = txtUrl.Text;
            await DownloadAndSaveMetadataAndImages();
            OnSave?.Invoke();

            LoadForm();

        }

        private void btnGenerateAudio_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (txtNarration.Text.IsEmpty()) return;

            EnsureFolders();

            using (var speechelo = new Speechelo())
            {
                SaveForm();
                speechelo.Setup();
                speechelo.GenerateNarration(_slideshowArticle.Narration,_slideshowArticle.NarrationAudioFilePath());

                Text = $"{_slideshowArticle.Id} {_slideshowArticle.Diagnostics()}";

                new Process { StartInfo = new ProcessStartInfo(_slideshowArticle.NarrationAudioFilePath()) { UseShellExecute = true } }.Start();
                this.Activate();
                OnSave?.Invoke();
            }
        }

        private void btnOpenFolder_Click(object sender, EventArgs e)
        {
            new Process { StartInfo = new ProcessStartInfo( _slideshowArticle.Folder()) { UseShellExecute = true } }.Start();
        }
    }
}
