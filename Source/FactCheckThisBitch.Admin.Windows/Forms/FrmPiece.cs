using FackCheckThisBitch.Common;
using FactCheckThisBitch.Admin.Windows.UserControls;
using FactCheckThisBitch.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace FactCheckThisBitch.Admin.Windows.Forms
{
    public partial class FrmPiece : Form
    {
        public Func<string,bool> OnMoveReferenceToOtherPiece;

        private Piece _piece;

        public FrmPiece(Piece piece)
        {
            _piece = piece;
            InitializeComponent();
        }

        private void FrmPieceEdit_Load(object sender, EventArgs e)
        {
            InitForm();
            LoadForm();
        }

        private void InitForm()
        {
            txtDuration.ValidationPattern = "^[1-9]?[0-9]$";
        }

        private void LoadForm()
        {
            this.Text = _piece.Title.ToSanitizedString();

            txtTitle.Text = _piece.Title;
            txtThesis.Text = _piece.Thesis;
            txtDuration.Text = _piece.Duration.ToString();
            txtKeywords.Text = string.Join(",", _piece.Keywords);

            LoadReferences();
        }

        private void SaveForm()
        {
            _piece.Title = txtTitle.Text.ValueOrNull();
            _piece.Thesis = txtThesis.Text.ValueOrNull();
            _piece.Keywords = txtKeywords.Text.Split(",").ToList();
            _piece.Duration = int.Parse(txtDuration.Text);

            foreach (TabPage tab in tabReferences.TabPages)
            {
                var referenceUi = tab.Controls[0] as ReferenceUi;
                referenceUi?.SaveForm();
            }
        }

        private void LoadReferences()
        {
            tabReferences.TabPages.Clear();

            for (var index = 0; index < _piece.References.Count; index++)
            {
                var reference = _piece.References[index];
                var tabPage = new TabPage()
                {
                    Tag = reference.Id,
                    Left = 0,
                    Top = 0,
                    Text = reference.Type.ToString(),
                    Width = tabReferences.Width,
                    Height = tabReferences.Height
                };

                var referenceUi = new ReferenceUi()
                {
                    Font = new Font(this.Font, FontStyle.Regular),
                    Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom,
                    Content = reference,
                    Width = tabReferences.Width,
                    Height = tabPage.Height,
                    AutoScaleMode = AutoScaleMode.None,
                    OnDelete = (string referenceId) =>
                    {
                        tabReferences.RemoveTabPage(referenceId);
                        _piece.References.Remove(_piece.References.First(r => r.Id == referenceId));
                        if (tabReferences.TabCount >= 1)
                            tabReferences.SelectedIndex = tabReferences.TabCount - 1;
                    },
                    OnMove = (string referenceId, int move) =>
                    {
                        var piece = _piece.References.First(r => r.Id == referenceId);
                        var pieceIndex = _piece.References.IndexOf(piece);
                        if (pieceIndex + move >= 0 && pieceIndex + move < _piece.References.Count)
                        {
                            _piece.References.Swap(pieceIndex, pieceIndex + move);
                            LoadReferences();
                        }
                    }, 
                    OnMoveToOtherPiece = (string referenceId) =>
                    {
                        if (OnMoveReferenceToOtherPiece!=null && OnMoveReferenceToOtherPiece.Invoke(referenceId))
                        {
                            DialogResult = DialogResult.OK;
                            Close();
                        };
                    }
                };
                tabPage.Controls.Add(referenceUi);

                tabReferences.TabPages.Add(tabPage);
            }
        }

        #region events

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveForm();

            DialogResult = DialogResult.OK;
            Close();
        }

        private async void btnGetArticleMetadata_Click(object sender, EventArgs e)
        {
            var reference = new Reference { Type = ReferenceType.Article };

            SaveForm();

            var url = Prompt.ShowDialog("Enter Url", "Add new reference");
            if (!url.IsEmpty())
            {
                var onlineArticleParser = new ArticleMetadataParser(url);
                IDictionary<string, string> metaData = default;
                try
                {
                    this.Cursor = Cursors.WaitCursor;
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
                    this.Cursor = Cursors.Default;
                }

                reference.Type = ReferenceType.Article;
                reference.Url = url;
                reference.Title = metaData.TryGet("title");
                reference.Description = metaData.TryGet("description");
                reference.Source = metaData.TryGet("site_name");
                reference.OriginalSource = metaData.TryGet("original-source");
                reference.Author = metaData.TryGet("author");

                if (DateTime.TryParse(metaData.TryGet("published_time"), out DateTime datePublished))
                {
                    reference.DatePublished = datePublished;
                }
                else if (DateTime.TryParse(metaData.TryGet("published"), out datePublished))
                {
                    reference.DatePublished = datePublished;
                }
                else if (DateTime.TryParse(metaData.TryGet("datePublished"), out datePublished))
                {
                    reference.DatePublished = datePublished;
                }

                string metadataKeywords = metaData.TryGet("keywords");
                if (metadataKeywords != null)
                {
                    var newKeywords = metadataKeywords.ToLower().Split(",").Take(5);
                    _piece.Keywords.AddRange(newKeywords.Where(k => !_piece.Keywords.Any(pk => pk == k)).ToList());
                }
            }

            _piece.References.Add(reference);

            LoadForm();

            tabReferences.SelectedIndex = tabReferences.TabCount - 1;
        }

        #endregion
    }
}