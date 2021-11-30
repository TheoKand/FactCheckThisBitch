using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using FactCheckThisBitch.Admin.Windows.Forms;
using FactCheckThisBitch.Models;

namespace FactCheckThisBitch.Admin.Windows.UserControls
{
    public partial class PuzzleUI : UserControl
    {
        private Puzzle _puzzle;
        public Puzzle Puzzle
        {
            get => _puzzle;
            set
            {
                if (value == null) return;

                _puzzle = value;

                ValidatePuzzle();
                LoadPieces();
            }
        }

        public PuzzleUI()
        {
            InitializeComponent();
        }



        private void PuzzleUI_Load(object sender, EventArgs e)
        {


        }

        private void LoadPieces()
        {
            this.SuspendLayout();

            Controls.Clear();

            if (_puzzle == null) return;

            const int leftMargin = 10;
            const int topMargin = 5;
            const int pieceWidth = 176;
            const int pieceHeight = 135;

            for (int x = 1; x <= Puzzle.Width; x++)
            {
                for (int y = 1; y <= Puzzle.Height; y++)
                {
                    var indexOfThisSquare = Puzzle.PieceIndexFromPosition(x, y);

                    var puzzlePiece = Puzzle.PuzzlePieces?.FirstOrDefault(p => p.Index == indexOfThisSquare);
                    var piece = puzzlePiece?.Piece;
                    if (piece == null)
                    {
                        piece = new Piece();
                        _puzzle.PuzzlePieces.Add(new PuzzlePiece
                        {
                            Index = indexOfThisSquare,
                            Piece = piece
                        });
                    }
                    var puzzlePieceX = leftMargin + (x - 1) * (pieceWidth + leftMargin);
                    var puzzlePieceY = topMargin + (y - 1) * (pieceHeight + topMargin);

                    PuzzlePieceUI puzzlePieceUi = new PuzzlePieceUI()
                    {
                        Piece = piece,
                        Name = piece.Id,
                        Left = puzzlePieceX,
                        Top = puzzlePieceY,
                    };
                    puzzlePieceUi.BackColor = puzzlePiece.Valid ? SystemColors.Control : Color.FromArgb(255, 192, 192);
                    puzzlePieceUi.OnClick = () => OnPieceClicked(piece);
                    puzzlePieceUi.OnDragDrop = OnPieceDragDrop;

                    Controls.Add(puzzlePieceUi);

                }
            }

            this.ResumeLayout();
        }

        private void ValidatePuzzle()
        {

            for (int x = 1; x <= Puzzle.Width; x++)
            {
                for (int y = 1; y <= Puzzle.Height; y++)
                {
                    var indexOfThisSquare = Puzzle.PieceIndexFromPosition(x, y);
                    var puzzlePiece = Puzzle.PuzzlePieces.First(p => p.Index == indexOfThisSquare);

                    if (puzzlePiece.Piece.Keywords.Length == 0 || puzzlePiece.Piece.Keywords.All(k=>string.IsNullOrWhiteSpace(k)))
                    {
                        puzzlePiece.Valid = false;
                        continue;
                    }

                    var connectsWith = new List<PuzzlePiece>();
                    if (y > 1) connectsWith.Add(Puzzle.PuzzlePieces.First(p => p.Index == Puzzle.PieceIndexFromPosition(x , y-1)));
                    if (x > 1) connectsWith.Add(Puzzle.PuzzlePieces.First(p => p.Index == Puzzle.PieceIndexFromPosition(x - 1, y)));
                    if (x < Puzzle.Width) connectsWith.Add(Puzzle.PuzzlePieces.First(p => p.Index == Puzzle.PieceIndexFromPosition(x +1, y)));
                    if (y < Puzzle.Height) connectsWith.Add(Puzzle.PuzzlePieces.First(p => p.Index == Puzzle.PieceIndexFromPosition(x , y+1)));

                    //check if it has at least one common keyword with all the pieces it connects with
                    bool atLeastOneConnectedPieceWithoutCommonKeywords = false;
                    foreach (var connectedPiece in connectsWith)
                    {
                        bool haveCommonKeyWords =
                            HaveAtLeastOneCommonKeyword(puzzlePiece.Piece.Keywords, connectedPiece.Piece.Keywords);

                        if (!haveCommonKeyWords)
                        {
                            atLeastOneConnectedPieceWithoutCommonKeywords = true;
                        }
                    }

                    puzzlePiece.Valid = !atLeastOneConnectedPieceWithoutCommonKeywords;

                }
            }

        }

        private bool HaveAtLeastOneCommonKeyword(string[] keywords1, string[] keywords2)
        {
            var nonEmptyKeywords1 = keywords1.Where(k => !string.IsNullOrWhiteSpace(k)).ToList();
            var nonEmptyKeywords2 = keywords2.Where(k => !string.IsNullOrWhiteSpace(k)).ToList();

            if (nonEmptyKeywords1.Count == 0 || nonEmptyKeywords2.Count == 0) return false;

            foreach (string keyword in nonEmptyKeywords1)
            {
                string allOfKeywords2 = string.Join(",", nonEmptyKeywords2);
                if (allOfKeywords2.Contains(keyword,
                    StringComparison.InvariantCultureIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        private void OnPieceDragDrop(string firstPieceId, string secondPieceId)
        {
            if (firstPieceId == secondPieceId) return;

            var firstPuzzlePiece = _puzzle.PuzzlePieces.First(p => p.Piece.Id == firstPieceId);
            var secondPuzzlePiece = _puzzle.PuzzlePieces.First(p => p.Piece.Id == secondPieceId);

            var secondPuzzlePieceIndex = secondPuzzlePiece.Index;
            secondPuzzlePiece.Index = firstPuzzlePiece.Index;
            firstPuzzlePiece.Index = secondPuzzlePieceIndex;

            ValidatePuzzle();
            LoadPieces();

        }

        private void OnPieceClicked(Piece piece)
        {
            FrmPiece pieceForm = new FrmPiece(piece);
            var result = pieceForm.ShowDialog();
            if (result != DialogResult.OK) return;

            (Controls.Find(piece.Id, true).First() as PuzzlePieceUI).Piece = piece;
            ValidatePuzzle();
            LoadPieces();
        }
    }
}
