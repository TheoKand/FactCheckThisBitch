using FactCheckThisBitch.Models;
using System;
using System.Diagnostics.Eventing.Reader;
using System.Windows.Forms;
using FackCheckThisBitch.Common;
using Newtonsoft.Json;
using System.IO;
using System.Text;

namespace FactCheckThisBitch.Admin.Windows.Forms
{
    public partial class FrmPuzzle : Form
    {
        private Puzzle _puzzle;
        private string _puzzleFileName;

        public FrmPuzzle()
        {
            InitializeComponent();
        }

        private void frmPuzzleMetadata_Load(object sender, EventArgs e)
        {
            InitForm();

            var lastPuzzle = UserSettings.Instance().LastPuzzle;
            if (!string.IsNullOrEmpty(lastPuzzle))
            {
                _puzzleFileName = lastPuzzle;
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
            txtSize.ValidationPattern = "^[3-9]x[3-9]$";
            txtSize.TextChanged = x => SizeChanged();
            puzzleUi.SaveToDisk = () => SaveToFile();
        }

        private void SizeChanged()
        {
            _puzzle.Width = int.Parse(txtSize.Text.Split('x')[0]);
            _puzzle.Height = int.Parse(txtSize.Text.Split('x')[1]);
            _puzzle.Resize();

            puzzleUi.Puzzle = _puzzle;
        }

        private void LoadForm()
        {
            UserSettings.Instance().LastPuzzle = _puzzleFileName;

            this.Text = _puzzle.Title.ToSanitizedString();

            txtTitle.Text = _puzzle.Title;
            txtThesis.Text = _puzzle.Thesis;
            txtSize.Text = $"{_puzzle.Width}x{_puzzle.Height}";

            puzzleUi.Puzzle = _puzzle;
        }

        private void CreateNew()
        {
            //TODO: Check for unsaved changes
            _puzzle = new Puzzle()
            {
                Title = $"New created at {DateTime.UtcNow}"
            };
            _puzzle.InitPieces();
            _puzzleFileName = Path.Combine(Configuration.Instance().DataFolder, $"{_puzzle.Title.ToSanitizedString()}.json");
            SaveToFile();
            LoadForm();
        }

        private void LoadFromFile()
        {
            var json = File.ReadAllText(_puzzleFileName, Encoding.UTF8);
            _puzzle = JsonConvert.DeserializeObject<Puzzle>(json, StaticSettings.JsonSerializerSettings);
            _puzzle.InitPieces();
        }

        private void SaveToFile()
        {
            var json = JsonConvert.SerializeObject(_puzzle, Formatting.Indented, StaticSettings.JsonSerializerSettings);
            File.WriteAllTextAsync(_puzzleFileName, json);
        }

        private void Save()
        {
            _puzzle.Title = txtTitle.Text;
            _puzzle.Thesis = txtThesis.Text;
            _puzzle.Width = int.Parse(txtSize.Text.Split('x')[0]);
            _puzzle.Height = int.Parse(txtSize.Text.Split('x')[1]);
        }

        #region Events
        private void btnOk_Click(object sender, EventArgs e)
        {
            Save();
            SaveToFile();
            MessageBox.Show("File saved ok");
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = Configuration.Instance().DataFolder;
            openFileDialog1.Title = "Open puzzle data file";
            openFileDialog1.DefaultExt = "json";
            openFileDialog1.CheckFileExists = true;
            openFileDialog1.CheckPathExists = true;
            openFileDialog1.ShowReadOnly = false;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                _puzzleFileName = openFileDialog1.FileName;
                LoadFromFile();
                LoadForm();
            }
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateNew();
        }

        private void FrmPuzzle_FormClosed(object sender, FormClosedEventArgs e)
        {
            Save();
            SaveToFile();
        }

        #endregion


    }
}
