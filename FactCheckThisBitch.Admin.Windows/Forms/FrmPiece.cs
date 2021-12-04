using FackCheckThisBitch.Common;
using FactCheckThisBitch.Admin.Windows.UserControls;
using FactCheckThisBitch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace FactCheckThisBitch.Admin.Windows.Forms
{
    public partial class FrmPiece : Form
    {
        private Piece _piece;
        private BaseContentUi _baseContentUi;
        private ContentUi _contentUi;
        private bool _loading;

        public FrmPiece(Piece piece)
        {
            _piece = piece;
            InitializeComponent();
        }

        private void FrmPieceEdit_Load(object sender, EventArgs e)
        {
            _loading = true;
            InitForm();
            LoadForm();
            _loading = false;
        }

        private void InitForm()
        {
            var pieceTypes = Enum.GetValues(typeof(PieceType)).Cast<PieceType>();
            cboType.DataSource = pieceTypes;
        }

        private void LoadForm()
        {
            this.Text = _piece.Title.ToSanitizedString();

            cboType.SelectedItem = _piece.Type;
            txtTitle.Text = _piece.Title;
            txtThesis.Text = _piece.Thesis;
            txtKeywords.Text = string.Join(", ", _piece.Keywords);
            imageEditor1.Images = _piece.Images.ToList();

            panelContent.Controls.Clear();
            LoadBaseContentUi();
            LoadContentUi();
        }

        private void SaveForm()
        {
            _piece.Title = txtTitle.Text;
            _piece.Thesis = txtThesis.Text;
            _piece.Keywords = txtKeywords.Text.ToLower().CommaSeparatedListToArray();
            _piece.Images = imageEditor1.Images.ToArray();
            _piece.Images = imageEditor1.Images.ToArray();
            _piece.Type = (PieceType) Enum.Parse(typeof(PieceType), cboType.SelectedValue.ToString() ?? string.Empty);

            _baseContentUi.SaveForm();
            _contentUi.SaveForm();

            _piece.Content = _baseContentUi.Content;
        }

        private void LoadBaseContentUi()
        {
            lblContent.Text = _piece.Type.ToString();

            _baseContentUi = new BaseContentUi();
            _baseContentUi.Content = _piece.Content;
            _baseContentUi.Left = 4;
            _baseContentUi.Top = 12;
            _baseContentUi.Width = panelContent.Width - 8;

            panelContent.Controls.Add(_baseContentUi);
        }

        private void LoadContentUi()
        {
            _contentUi = new ContentUi();
            _contentUi.Content = _piece.Content;
            _contentUi.Left = 4;
            _contentUi.Top = _baseContentUi.Bottom + 20;
            _contentUi.Width = _baseContentUi.Width - 8;
            _contentUi.Height = _piece.Content.PropertiesNotFromInterface().Count() * 30 + 50;

            panelContent.Controls.Add(_contentUi);
        }

        #region events

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cboType_SelectedValueChanged(object sender, EventArgs e)
        {
            if (_loading) return;
            var newType = (PieceType) Enum.Parse(typeof(PieceType), cboType.SelectedValue.ToString() ?? string.Empty);
            _piece.Type = newType;
            _piece.ConvertContentToNewTypeAndKeepMetadata();

            panelContent.Controls.Clear();
            LoadBaseContentUi();
            LoadContentUi();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveForm();

            DialogResult = DialogResult.OK;
            Close();
        }

        private async void btnGetArticleMetadata_Click(object sender, EventArgs e)
        {
            SaveForm();

            if (_piece.Content.Url.IsEmpty()) return;

            var onlineArticleParser = new ArticleMetadataParser(_piece.Content.Url);
            IDictionary<string, string> metaData = default;
            try
            {
                Cursor.Current = Cursors.WaitCursor;
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
            }

            _piece.Content.Title = metaData.TryGet("title");
            _piece.Content.Summary = metaData.TryGet("description");
            _piece.Content.Source = metaData.TryGet("site_name");
            _piece.Content.References = metaData.TryGet("original-source")?.Split(",");

            if (DateTime.TryParse(metaData.TryGet("published_time"), out DateTime datePublished))
            {
                _piece.Content.DatePublished = datePublished;
            }
            else if (DateTime.TryParse(metaData.TryGet("published"), out datePublished))
            {
                _piece.Content.DatePublished = datePublished;
            }
            else if (DateTime.TryParse(metaData.TryGet("datePublished"), out datePublished))
            {
                _piece.Content.DatePublished = datePublished;
            }

            string metadataKeywords = metaData.TryGet("keywords");
            if (_piece.Keywords.Length == 0 && metadataKeywords != null)
            {
                var newKeywords = metadataKeywords.ToLower().Split(",").Take(5);
                var pieceKeywords = _piece.Keywords.ToList();
                pieceKeywords.AddRange(newKeywords.Where(k => !pieceKeywords.Any(pk => pk == k)).ToList());
                _piece.Keywords = pieceKeywords.ToArray();
            }

            if (_piece.Content is Article)
            {
                (_piece.Content as Article).Author = metaData.TryGet("author");
            }

            LoadForm();
        }

        #endregion
    }
}