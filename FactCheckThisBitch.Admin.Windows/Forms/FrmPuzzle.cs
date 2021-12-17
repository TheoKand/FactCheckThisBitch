﻿using FackCheckThisBitch.Common;
using FactCheckThisBitch.Models;
using FactCheckThisBitch.Render;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FactCheckThisBitch.Admin.Windows.Forms
{
    public partial class FrmPuzzle : Form
    {
        private Puzzle _puzzle;
        private bool _isDirty = false;

        public bool IsDirty
        {
            get => _isDirty;
            set
            {
                _isDirty = value;
                this.Text = $"{_puzzle.Title.ToSanitizedString()} {(_isDirty ? " - Modified" : "")}";
            }
        }

        public FrmPuzzle()
        {
            InitializeComponent();
        }

        private void frmPuzzleMetadata_Load(object sender, EventArgs e)
        {
            InitForm();
            if (!string.IsNullOrEmpty(UserSettings.Instance().CurrentPuzzle) &&
                File.Exists(UserSettings.Instance().CurrentPuzzlePath))
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
            txtDuration.ValidationPattern = "^[1-9]?[0-9]$";
            txtDuration.TextChanged = x => IsDirty = true;
            txtSize.ValidationPattern = "^[3-9]x[3-9]$";
            txtSize.TextChanged = x => SizeChanged();
            puzzleUi.SaveToFile = SaveToFile;
            puzzleUi.OnChanged = () => IsDirty = true;
        }

        private new void SizeChanged()
        {
            _puzzle.Width = int.Parse(txtSize.Text.Split('x')[0]);
            _puzzle.Height = int.Parse(txtSize.Text.Split('x')[1]);
            puzzleUi.Puzzle = _puzzle;
            IsDirty = true;
        }

        private void LoadForm()
        {
            UserSettings.Instance().CurrentPuzzle = _puzzle.FileName;
            this.Text = _puzzle.Title.ToSanitizedString();
            txtTitle.Text = _puzzle.Title;
            txtThesis.Text = _puzzle.Thesis;
            txtConclusion.Text = _puzzle.Conclusion;
            txtSize.Text = $"{_puzzle.Width}x{_puzzle.Height}";
            txtDuration.Text = _puzzle.Duration.ToString();
            puzzleUi.Puzzle = _puzzle;
            IsDirty = false;
        }

        private void CreateNew()
        {
            if (IsDirty)
            {
                var result = MessageBox.Show("Do you want to save your changes?", "Save Changes", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    SaveToFile();
                }
            }

            _puzzle = new Puzzle() {Title = $"New created at {DateTime.UtcNow}"};
        }

        private void LoadFromFile()
        {
            var json = File.ReadAllText(UserSettings.Instance().CurrentPuzzlePath, Encoding.UTF8);
            _puzzle = JsonConvert.DeserializeObject<Puzzle>(json, StaticSettings.JsonSerializerSettings);
        }

        private void SaveToFile()
        {
            var json = JsonConvert.SerializeObject(_puzzle, Formatting.Indented, StaticSettings.JsonSerializerSettings);
            File.WriteAllTextAsync(UserSettings.Instance().CurrentPuzzlePath, json);
            IsDirty = false;
        }

        private void Save()
        {
            _puzzle.Title = txtTitle.Text.ValueOrNull();
            _puzzle.Thesis = txtThesis.Text.ValueOrNull();
            _puzzle.Conclusion = txtConclusion.Text.ValueOrNull();
            _puzzle.Width = int.Parse(txtSize.Text.Split('x')[0]);
            _puzzle.Height = int.Parse(txtSize.Text.Split('x')[1]);
            _puzzle.Duration = int.Parse( txtDuration.Text);
            CheckRenameFile();
        }

        private void CheckRenameFile()
        {
            if (_puzzle.FileName != UserSettings.Instance().CurrentPuzzle)
            {
                UserSettings.Instance().CurrentPuzzle = _puzzle.FileName;
                this.Text = _puzzle.Title.ToSanitizedString();
            }
        }

        #region Events

        private void FrmPuzzle_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (IsDirty)
            {
                var result = MessageBox.Show("Do you want to save your changes?", "Save Changes", MessageBoxButtons.YesNo);
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

        private void txtThesis_TextChanged(object sender, EventArgs e)
        {
            IsDirty = true;
        }

        private void txtConclusion_TextChanged(object sender, EventArgs e)
        {
            IsDirty = true;
        }

        private void newToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            CreateNew();
            LoadForm();
        }

        private void openToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = Configuration.Instance().DataFolder;
            openFileDialog1.Title = "Open puzzle data file";
            openFileDialog1.DefaultExt = "json";
            openFileDialog1.CheckFileExists = true;
            openFileDialog1.CheckPathExists = true;
            openFileDialog1.ShowReadOnly = false;
            if (openFileDialog1.ShowDialog() != DialogResult.OK) return;
            UserSettings.Instance().CurrentPuzzle = openFileDialog1.FileName;
            LoadFromFile();
            LoadForm();
        }

        private void saveToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Save();
            SaveToFile();
        }

        private void renderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                Cursor.Current = Cursors.WaitCursor;

                string puzzleOutputFolder =
                    Path.Combine(Configuration.Instance().OutputFolder, _puzzle.Title.ToSanitizedString());

                using (var renderer = new PuzzleRenderer(_puzzle, Configuration.Instance().AssetsFolder, puzzleOutputFolder,Path.Combine(Configuration.Instance().DataFolder, "media")))
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

        private void getDescriptionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var desc = _puzzle.ToDescription();

            Clipboard.SetText(desc);
            MessageBox.Show("Copied to clipboard");
        }

        private void FrmPuzzle_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2)
            {
                Save();
                SaveToFile();
            }
        }


        #endregion


    }
}