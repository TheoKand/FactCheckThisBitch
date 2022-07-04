using FackCheckThisBitch.Common;
using FactCheckThisBitch.Render;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Windows.Forms;
using VideoFromArticle.Models;
using WebAutomation;

namespace VideoFromArticle.Admin.Windows.Forms
{
    public partial class FrmSlideshow : Form
    {
        private Slideshow _slideshow;
        private bool _isDirty;

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
                _slideshow = LoadFromFile(UserSettings.Instance().CurrentSlideshow.FilePath());
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
                var fromArticle = ((ArticleListBoxItem)args.Data.GetData(typeof(ArticleListBoxItem))).Article;
                var fromIndex = _slideshow.Articles.IndexOf(fromArticle);
                var toIndex = lstArticles.IndexFromScreenPoint(new Point(args.X, args.Y));
                if (fromIndex == toIndex || toIndex == -1 || fromIndex == -1) return;

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

        private static Slideshow LoadFromFile(string file)
        {
            var json = File.ReadAllText(file, Encoding.UTF8);
            return JsonConvert.DeserializeObject<Slideshow>(json, StaticSettings.JsonSerializerSettings);
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
                totalSeconds += article.SlideDurationInSeconds();
            }

            var timeSpan = TimeSpan.FromSeconds(totalSeconds);

            lblTotals.Text = $"Duration: {timeSpan:mm\\:ss\\:FF}";
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
            _slideshow = LoadFromFile(openFileDialog1.FileName);
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
            using var articleForm = new FrmArticle(newArticle)
            {
                OnSave = () => { saveToolStripMenuItem_Click(null, null); }
            };
            articleForm.ShowDialog();
            _slideshow.Articles.Add(newArticle);
            IsDirty = true;
            LoadArticles();
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
            using var articleForm = new FrmArticle(article)
            {
                OnSave = () => { saveToolStripMenuItem_Click(null, null); }
            };
            var articleBefore = JsonConvert.SerializeObject(article, StaticSettings.JsonSerializerSettings);
            articleForm.ShowDialog();
            articleForm.Dispose();
            var articleAfter = JsonConvert.SerializeObject(article, StaticSettings.JsonSerializerSettings);
            if (articleBefore != articleAfter)
            {
                IsDirty = true;
                LoadArticles();
            }
        }

        private void btnRender_Click(object sender, EventArgs e)
        {
            if (_slideshow.Articles.Count == 0 || _slideshow.Title.IsEmpty())
            {
                return;
            }

            var optionsForm = new FrmRenderOptions();
            var result = optionsForm.ShowDialog();
            if (result != DialogResult.OK)
                return;

            try
            {
                this.Cursor = Cursors.WaitCursor;
                Cursor.Current = Cursors.WaitCursor;
                Application.DoEvents();

                using var renderer = new VideoFromArticleRenderer(_slideshow, optionsForm.Options.Template,
                    optionsForm.Options.IntroDurationSeconds,
                    _slideshow.Folder(), Configuration.Instance().VideoFromArticleTempatesFolder);
                renderer.Render();
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
            Speechelo speechelo = null;
            try
            {
                var articlesWithNarration = _slideshow.Articles.ToList();

                for (var i = 0; i < articlesWithNarration.Count; i++)
                {
                    var article = articlesWithNarration[i];
                    var voice = StaticSettings.AvailableVoices.ToList()[i % StaticSettings.AvailableVoices.Count]
                        .Key;

                    foreach (var articleImage in article.Images)
                    {
                        var narration = articleImage.Narration;

                        if (narration.IsNotEmpty())
                        {
                            var imageNarrationFilePath = article.ImageNarrationAudioFilePath(articleImage);
                            if (!File.Exists(imageNarrationFilePath))
                            {
                                //TODO: create speechelo wrapper
                                if (speechelo == null)
                                {
                                    speechelo = new Speechelo();
                                    speechelo.Setup();
                                }
                                speechelo.GenerateNarration(narration, voice, imageNarrationFilePath);
                            }

                            articleImage.AudioDuration = article.GetDurationFromNarrationAudioFile(articleImage).TotalSeconds;
                        }
                    }
                    //TODO: each slide duration was calculated here, and the sum would go to Article Duration. Do this elsewhere

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
            var slideshowOutputFolder =
                Path.Combine(Configuration.Instance().VideoFromArticleTempatesFolder, "_output",
                    _slideshow.SanitizedTitle);
            new Process { StartInfo = new ProcessStartInfo(slideshowOutputFolder) { UseShellExecute = true } }.Start();
        }

        private void compactFileSystemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //delete files not used from anywhere in the file system. Includes unused article folders and unused images inside used articles

            var deleteDirectories = new List<string>();
            var deleteFiles = new List<string>();

            var dataFolderDirectory = new DirectoryInfo(Configuration.Instance().DataFolder);
            foreach (var slideshowDirectory in dataFolderDirectory.GetDirectories())
            {
                var slideshowFile = Path.Combine(slideshowDirectory.FullName, "slideshow.json");
                var slideshow = LoadFromFile(slideshowFile);

                foreach (var articleFolder in slideshowDirectory.GetDirectories().Where(d => Guid.TryParse(d.Name, out _)))
                {
                    var articleIdFromArticleFolder = articleFolder.Name;
                    var article = slideshow.Articles.FirstOrDefault(a => a.Id == articleIdFromArticleFolder);
                    if (article == null)
                    {
                        deleteDirectories.Add(articleFolder.FullName);
                    }
                    else
                    {
                        foreach (var imageFile in articleFolder.GetFiles().Where(f =>
                                     f.Extension.ToLower() == ".png" || f.Extension.ToLower() == ".jpg"))
                        {
                            var image = article.Images.FirstOrDefault(i => i.Filename == imageFile.Name);
                            if (image == null)
                            {
                                deleteFiles.Add(imageFile.FullName);
                            }
                        }
                    }
                }
            }

            var reply = MessageBox.Show($"Delete {deleteDirectories.Count} directories and {deleteFiles.Count} files?",
                "Confirmation", MessageBoxButtons.OKCancel);
            if (reply == DialogResult.OK)
            {
                try
                {

                    foreach (var deleteDirectory in deleteDirectories)
                    {
                        Directory.Delete(deleteDirectory, true);
                    }

                    foreach (var deleteFile in deleteFiles)
                    {
                        File.Delete(deleteFile);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }

            }

        }
    }
}