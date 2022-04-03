using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Windows.Forms;
using FackCheckThisBitch.Common;
using FactCheckThisBitch.Render;
using Newtonsoft.Json;
using VideoFromArticle.Models;
using WebAutomation;

namespace VideoFromArticle.Admin.Windows.Forms
{
    public partial class FrmSlideshow : Form
    {
        private Slideshow _slideshow;
        private bool _isDirty = false;

        public bool IsDirty
        {
            get => _isDirty;
            set
            {
                _isDirty = value;
                this.Text = $"{_slideshow.SanitizedTitle} {(_isDirty ? " - Modified" : "")}";
            }
        }

        public FrmSlideshow()
        {
            InitializeComponent();
        }

        private void frmSlideshow_Load(object sender, EventArgs e)
        {
            InitForm();

            if (UserSettings.Instance().CurrentSlideshow != null &&
                File.Exists(UserSettings.Instance().CurrentSlideshow.FilePath()))
            {
                LoadFromFile(UserSettings.Instance().CurrentSlideshow.FilePath());
                LoadForm();
            }
        }

        private void InitForm()
        {
            lstArticles.AllowDrop = true;
            lstArticles.MouseDown += (sender, args) =>
            {
                if (lstArticles.SelectedItem == null) return;
                if (args.Button == MouseButtons.Left && args.Clicks == 1)
                {
                    lstArticles.DoDragDrop(lstArticles.SelectedItem, DragDropEffects.All);
                }
            };
            lstArticles.DragEnter += (sender, args) => { args.Effect = DragDropEffects.All; };
            lstArticles.DragDrop += (sender, args) =>
            {
                var fromArticle = ((ArticleListBoxItem) args.Data.GetData(typeof(ArticleListBoxItem))).Article;
                var fromIndex = _slideshow.Articles.IndexOf(fromArticle);
                var toIndex = lstArticles.IndexFromScreenPoint(new Point(args.X, args.Y));
                if (fromIndex == toIndex) return;

                _slideshow.Articles.Remove(fromArticle);
                _slideshow.Articles.Insert(toIndex, fromArticle);

                LoadArticles();
                lstArticles.SelectedIndex = toIndex;
                IsDirty = true;
            };
            panelSlideshow.Visible = false;
        }

        private void LoadForm()
        {
            this.Text = _slideshow.SanitizedTitle;
            txtId.Text = _slideshow.Id;
            txtTitle.Text = _slideshow.Title;
            LoadArticles();
            IsDirty = false;

            panelSlideshow.Visible = true;
        }

        private Slideshow LoadFromFile(string file)
        {
            var json = File.ReadAllText(file, Encoding.UTF8);
            _slideshow = JsonConvert.DeserializeObject<Slideshow>(json, StaticSettings.JsonSerializerSettings);
            return _slideshow;
        }

        private bool FileIsUpToDate()
        {
            var fileJson = File.ReadAllText(UserSettings.Instance().CurrentSlideshow.FilePath(), Encoding.UTF8);
            var formJson = JsonConvert.SerializeObject(_slideshow,
                Formatting.Indented, StaticSettings.JsonSerializerSettings);
            return formJson == fileJson;
        }

        private void SaveToFile()
        {
            var json = JsonConvert.SerializeObject(_slideshow, Formatting.Indented,
                StaticSettings.JsonSerializerSettings);

            if (!Directory.Exists(UserSettings.Instance().CurrentSlideshow.Folder()))
            {
                Directory.CreateDirectory(UserSettings.Instance().CurrentSlideshow.Folder());
            }

            File.WriteAllTextAsync(UserSettings.Instance().CurrentSlideshow.FilePath(), json);
            IsDirty = false;
        }

        private bool CreateNew()
        {
            if (IsDirty)
            {
                var result = MessageBox.Show("Do you want to save your changes?", "Save Changes",
                    MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    SaveToFile();
                }
            }

            var newFileName = Prompt.ShowDialog("Enter Slideshow Title", "Slideshow Title",
                $"Slideshow created at {DateTime.UtcNow}");
            if (newFileName.IsEmpty()) return false;
            _slideshow = new Slideshow() { Title = newFileName };
            return true;
        }

        private void Save()
        {
            _slideshow.Title = txtTitle.Text.ValueOrNull();
        }

        private void LoadArticles()
        {
            double totalSeconds = 0;
            lstArticles.Items.Clear();
            foreach (var article in _slideshow.Articles)
            {
                lstArticles.Items.Add(new ArticleListBoxItem(article));
                totalSeconds += article.ProjectedDurationInSeconds();
            }

            var timeSpan = TimeSpan.FromSeconds(totalSeconds);

            lblTotals.Text = $"Duration: {timeSpan.ToString("mm\\:ss\\:FF")}";
        }

        private void frmSlideshow_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (IsDirty)
            {
                var result = MessageBox.Show("Do you want to save your changes?", "Save Changes",
                    MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    Save();
                    SaveToFile();
                }
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = Configuration.Instance().DataFolder;
            openFileDialog1.Title = "Open file";
            openFileDialog1.DefaultExt = "json";
            openFileDialog1.CheckFileExists = true;
            openFileDialog1.CheckPathExists = true;
            openFileDialog1.ShowReadOnly = false;
            if (openFileDialog1.ShowDialog() != DialogResult.OK || openFileDialog1.FileName.IsEmpty()) return;
            UserSettings.Instance().CurrentSlideshow = LoadFromFile(openFileDialog1.FileName);
            UserSettings.Instance().Save();
            LoadForm();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Save();
            SaveToFile();
        }

        private void frmSlideshow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2)
            {
                Save();
                SaveToFile();
            }
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CreateNew())
            {
                UserSettings.Instance().CurrentSlideshow = _slideshow;
                UserSettings.Instance().Save();

                LoadForm();
            }
        }

        private void btnAddArticle_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var url = Prompt.ShowDialog("Enter Url", "Add new article");
            var newArticle = new Article() { Url = url, SlideshowFolder = _slideshow.Folder() };
            using (var articleForm = new FrmArticle(newArticle)
                   {
                       OnSave = () => { saveToolStripMenuItem_Click(null, null); }
                   })
            {
                articleForm.ShowDialog();
                _slideshow.Articles.Add(newArticle);
                IsDirty = true;
                LoadArticles();
            }
        }

        private void btnDelete_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var article = (lstArticles.SelectedItem as ArticleListBoxItem)?.Article;
            if (article == null) return;
            _slideshow.Articles.Remove(article);
            IsDirty = true;
            lstArticles.Items.Remove(lstArticles.SelectedItem);
        }

        private void lstArticles_DoubleClick(object sender, EventArgs e)
        {
            var article = (lstArticles.SelectedItem as ArticleListBoxItem)?.Article;
            if (article == null) return;
            using (FrmArticle articleForm = new FrmArticle(article)
                   {
                       OnSave = () => { saveToolStripMenuItem_Click(null, null); }
                   })
            {
                var articleBefore = JsonConvert.SerializeObject(article, StaticSettings.JsonSerializerSettings);
                var result = articleForm.ShowDialog();
                var articleAfter = JsonConvert.SerializeObject(article, StaticSettings.JsonSerializerSettings);
                if (articleBefore != articleAfter)
                {
                    IsDirty = true;
                    LoadArticles();
                }
            }
        }

        private void lstArticles_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void btnRender_Click(object sender, EventArgs e)
        {
            //TODO: validation
            if (_slideshow.Articles.Count == 0 || _slideshow.Title.IsEmpty())
            {
                return;
            }

            FrmRenderOptions optionsForm = new FrmRenderOptions();
            var result = optionsForm.ShowDialog();
            if (result != DialogResult.OK)
                return;

            try
            {
                this.Cursor = Cursors.WaitCursor;
                Cursor.Current = Cursors.WaitCursor;
                Application.DoEvents();

                using (var renderer = new VideoFromArticleRenderer(_slideshow, optionsForm.Options.Template,
                           optionsForm.Options.IntroDurationSeconds,
                           _slideshow.Folder(), Configuration.Instance().VideoFromArticleTempatesFolder))
                {
                    renderer.Render();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                this.Cursor = Cursors.Default;
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnGenerateNarrations_Click(object sender, EventArgs e)
        {
            using (var speechelo = new Speechelo())
            {
                try
                {
                    speechelo.Setup();

                    var articlesWithNarration = _slideshow.Articles
                        .Where(_ => _.Narration.IsNotEmpty() || _.NarrationPerImage.Value)
                        .ToList();
                    for (var i = 0; i < articlesWithNarration.Count(); i++)
                    {
                        var article = articlesWithNarration[i];
                        var voice = StaticSettings.AvailableVoices.ToList()[i % StaticSettings.AvailableVoices.Count]
                            .Key;

                        if (article.NarrationPerImage.Value)
                        {
                            for (var articleImageIndex = 0;
                                 articleImageIndex < article.Images.Count;
                                 articleImageIndex++)
                            {
                                var articleImage = article.Images[articleImageIndex];

                                var (narration, duration) = articleImage.Narration.ParseNarration();

                                if (narration.IsNotEmpty())
                                {
                                    var imageNarrationFilePath = article.ImageNarrationAudioFilePath(articleImage);
                                    if (!File.Exists(imageNarrationFilePath))
                                    {
                                        speechelo.GenerateNarration(narration, voice, imageNarrationFilePath);
                                    }

                                    articleImage.AudioDuration =
                                        duration ?? article.ReadNarrationDuration(articleImage).TotalSeconds;
                                    if (!articleImage.Group)
                                    {
                                        articleImage.SlideDurationInSeconds = articleImage.AudioDuration;
                                    }
                                }
                            }

                            var groupedImages = article.Images.Where(_ => _.Group).ToList();
                            if (groupedImages.Any())
                            {
                                List<ArticleImage> currentGroup = new List<ArticleImage>() { groupedImages.First() };
                                for (var imageIndex = 1; imageIndex < groupedImages.Count; imageIndex++)
                                {
                                    var articleImage = groupedImages[imageIndex];

                                    if (articleImage.Narration.IsNotEmpty())
                                    {
                                        var durationForEachImageOfPreviousGroup =
                                            currentGroup.First().AudioDuration / currentGroup.Count;
                                        currentGroup.ForEach(_ =>
                                            _.SlideDurationInSeconds = durationForEachImageOfPreviousGroup);

                                        currentGroup.Clear();
                                        currentGroup.Add(articleImage);
                                    }
                                    else
                                    {
                                        currentGroup.Add(articleImage);

                                        if (imageIndex == groupedImages.Count - 1)
                                        {
                                            var durationForEachImageOfPreviousGroup =
                                                currentGroup.First().AudioDuration / currentGroup.Count;
                                            currentGroup.ForEach(_ =>
                                                _.SlideDurationInSeconds = durationForEachImageOfPreviousGroup);
                                        }
                                    }
                                }
                            }

                            article.DurationInSeconds = article.Images.Sum(_ => _.SlideDurationInSeconds);
                        }
                        else
                        {
                            article.Narration = article.Narration.SanitizeNarration();
                            var (narration, duration) = article.Narration.ParseNarration();
                            speechelo.GenerateNarration(narration, voice, article.ArticleNarrationAudioFilePath());
                            article.DurationInSeconds = duration ?? article.ReadNarrationDuration().TotalSeconds;
                        }

                        saveToolStripMenuItem_Click(null, null);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
                finally
                {
                    LoadArticles();
                    SystemSounds.Beep.Play();
                    this.Activate();
                }
            }
        }

        private void btnSortByDate_Click(object sender, EventArgs e)
        {
            _slideshow.Articles = _slideshow.Articles.OrderBy(_ => _.Published).ToList();
            LoadArticles();
            IsDirty = true;
            saveToolStripMenuItem_Click(null, null);
        }

        private void btnOpenFolder_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(_slideshow.Folder())) return;
            new Process { StartInfo = new ProcessStartInfo(_slideshow.Folder()) { UseShellExecute = true } }.Start();
        }

        private void btnRefresh_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            LoadArticles();
        }

        private void btnOutputFolder_Click(object sender, EventArgs e)
        {
            string slideshowOutputFolder =
                Path.Combine(Configuration.Instance().VideoFromArticleTempatesFolder, "_output",
                    _slideshow.SanitizedTitle);
            new Process { StartInfo = new ProcessStartInfo(slideshowOutputFolder) { UseShellExecute = true } }.Start();
        }
    }
}