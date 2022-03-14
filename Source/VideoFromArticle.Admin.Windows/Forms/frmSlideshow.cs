using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using FackCheckThisBitch.Common;
using Newtonsoft.Json;
using VideoFromArticle.Models;

namespace VideoFromArticle.Admin.Windows.Forms
{
    public partial class FrmSlideshow : Form
    {
        private Slideshow _slideshow;
        private bool _isDirty = false;

        public bool IsDirty
        {
            get =>
                _isDirty;
            set
            {
                _isDirty = value;
                this.Text = $"{_slideshow.FullTitle} {(_isDirty ? " - Modified" : "")}";
            }
        }

        public FrmSlideshow()
        {
            InitializeComponent();
        }

        private void frmSlideshow_Load(object sender, EventArgs e)
        {
            InitForm();
            if (!string.IsNullOrEmpty(UserSettings.Instance()
                    .CurrentFile) &&
                File.Exists(UserSettings.Instance()
                    .CurrentFilePath))
            {
                LoadFromFile();
                LoadForm();
            }
            else
            {
                CreateNew();
                LoadForm();
            }
        }

        private void InitForm()
        {
            lstArticles.AllowDrop = true;
            lstArticles.MouseDown += (sender, args) =>
            {
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

                _slideshow.Articles.Swap(fromIndex, toIndex);
                LoadArticles();
                lstArticles.SelectedIndex = toIndex;
                
                IsDirty = true;
            };
        }

        private void LoadForm()
        {
            UserSettings.Instance()
                .CurrentFile = _slideshow.FileName;
            this.Text = _slideshow.FullTitle;

            txtId.Text = _slideshow.Id;
            txtTitle.Text = _slideshow.Title;

            LoadArticles();

            IsDirty = false;
        }

        private void LoadFromFile()
        {
            var json = File.ReadAllText(UserSettings.Instance()
                    .CurrentFilePath,
                Encoding.UTF8);
            _slideshow = JsonConvert.DeserializeObject<Slideshow>(json,
                StaticSettings.JsonSerializerSettings);
        }

        private bool FileIsUpToDate()
        {
            var fileJson = File.ReadAllText(UserSettings.Instance()
                    .CurrentFilePath,
                Encoding.UTF8);
            var formJson = JsonConvert.SerializeObject(_slideshow,
                Formatting.Indented,
                StaticSettings.JsonSerializerSettings);

            return formJson == fileJson;
        }

        private void SaveToFile()
        {
            var json = JsonConvert.SerializeObject(_slideshow,
                Formatting.Indented,
                StaticSettings.JsonSerializerSettings);
            File.WriteAllTextAsync(UserSettings.Instance()
                    .CurrentFilePath,
                json);
            IsDirty = false;
        }

        private void CreateNew()
        {
            if (IsDirty)
            {
                var result = MessageBox.Show("Do you want to save your changes?",
                    "Save Changes",
                    MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    SaveToFile();
                }
            }

            _slideshow = new Slideshow() { Title = $"Slideshow created at {DateTime.UtcNow}" };
        }

        private void Save()
        {
            _slideshow.Title = txtTitle.Text.ValueOrNull();

            CheckRenameFile();
        }

        private void CheckRenameFile()
        {
            if (_slideshow.FileName !=
                UserSettings.Instance()
                    .CurrentFile)
            {
                UserSettings.Instance()
                    .CurrentFile = _slideshow.FileName;
                this.Text = _slideshow.FullTitle;
            }
        }

        private void LoadArticles()
        {
            lstArticles.Items.Clear();
            foreach (var article in _slideshow.Articles)
            {
                lstArticles.Items.Add(new ArticleListBoxItem(article));
            }
        }

        private void frmSlideshow_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (IsDirty)
            {
                var result = MessageBox.Show("Do you want to save your changes?",
                    "Save Changes",
                    MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    SaveToFile();
                }
            }
        }

        private void txtTitle_TextChanged(object sender, EventArgs e)
        {
            IsDirty = true;
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = Configuration.Instance()
                .DataFolder;
            openFileDialog1.Title = "Open file";
            openFileDialog1.DefaultExt = "json";
            openFileDialog1.CheckFileExists = true;
            openFileDialog1.CheckPathExists = true;
            openFileDialog1.ShowReadOnly = false;
            if (openFileDialog1.ShowDialog() != DialogResult.OK)
                return;
            UserSettings.Instance()
                .CurrentFile = openFileDialog1.FileName;
            LoadFromFile();
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
            CreateNew();
            LoadForm();
        }

        private void btnAddArticle_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var url = Prompt.ShowDialog("Enter Url", "Add new article");

            var newArticle = new SlideshowArticle() { Url = url };

            using (var articleForm = new FrmArticle(newArticle)
                   {
                       OnSave = () =>
                       {
                           saveToolStripMenuItem_Click(null, null);
                       }
                   })
            {
                var result = articleForm.ShowDialog();
                if (result != DialogResult.OK) return;

                _slideshow.Articles.Add( newArticle);
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

            using (FrmArticle articleForm = new FrmArticle(article))
            {
                var articleBefore = JsonConvert.SerializeObject(article, StaticSettings.JsonSerializerSettings);

                var result = articleForm.ShowDialog();
                if (result != DialogResult.OK) return;

                var articleAfter = JsonConvert.SerializeObject(article, StaticSettings.JsonSerializerSettings);

                if (articleBefore != articleAfter)
                {
                    IsDirty = true;
                    LoadArticles();
                }
            }
            
        }
    }
}
