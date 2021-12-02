using FactCheckThisBitch.Admin.Windows.Forms;
using FactCheckThisBitch.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace FactCheckThisBitch.Admin.Windows.UserControls
{
    public partial class PuzzleUi : UserControl
    {
        private Puzzle _puzzle;

        public Action SaveToDisk;

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

        public PuzzleUi()
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

            const int leftMargin = 50;
            const int topMargin = 50;
            const int pieceWidth = 194;
            const int pieceHeight = 170;

            for (int x = 1; x <= Puzzle.Width; x++)
            {
                for (int y = 1; y <= Puzzle.Height; y++)
                {
                    var indexOfThisSquare = Puzzle.PieceIndexFromPosition(x, y);

                    var puzzlePiece = Puzzle.PuzzlePieces?.First(p => p.Index == indexOfThisSquare);
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

                    PuzzlePieceUi puzzlePieceUi = new PuzzlePieceUi()
                    {
                        Piece = piece,
                        Name = piece.Id,
                        Left = puzzlePieceX,
                        Top = puzzlePieceY,
                    };
                    puzzlePieceUi.BackColor = puzzlePiece != null && puzzlePiece.Valid
                        ? Color.LightGreen
                        : Color.FromArgb(255, 192, 192);
                    puzzlePieceUi.OnClick = () => OnPieceClicked(piece);
                    puzzlePieceUi.OnDragDrop = OnPieceDragDrop;

                    Controls.Add(puzzlePieceUi);
                }
            }

            this.ResumeLayout();
        }

        private bool PieceHasAllValidNeighbours(Piece piece, List<PuzzlePiece> neighbours)
        {

            if (piece.Keywords.Length == 0 ||
                piece.Keywords.All(string.IsNullOrWhiteSpace))
            {
                return false;
            }

            bool atLeastOneConnectedPieceWithoutCommonKeywords = false;
            foreach (var connectedPiece in neighbours)
            {
                bool haveCommonKeyWords =
                    HaveAtLeastOneCommonKeyword(piece.Keywords, connectedPiece.Piece.Keywords);

                if (!haveCommonKeyWords)
                {
                    atLeastOneConnectedPieceWithoutCommonKeywords = true;
                }
            }

            return !atLeastOneConnectedPieceWithoutCommonKeywords;
        }

        private bool PieceHasEnoughValidNeighbours(Piece piece, List<PuzzlePiece> neighbours)
        {

            if (piece.Keywords.Length == 0 ||
                piece.Keywords.All(string.IsNullOrWhiteSpace))
            {
                return false;
            }

            var maxInvalidNeighbours = 1;
            if (neighbours.Count == 4)
            {
                maxInvalidNeighbours = 2;
            }
            else if (neighbours.Count == 3)
            {
                maxInvalidNeighbours = 1;
            }

            int invalidNeighbours = 0;
            foreach (var connectedPiece in neighbours)
            {
                bool haveCommonKeyWords =
                    HaveAtLeastOneCommonKeyword(piece.Keywords, connectedPiece.Piece.Keywords);

                if (!haveCommonKeyWords)
                {
                    invalidNeighbours++;
                }
            }

            return invalidNeighbours <= maxInvalidNeighbours;

        }


        private void ValidatePuzzle()
        {
            for (int x = 1; x <= Puzzle.Width; x++)
            {
                for (int y = 1; y <= Puzzle.Height; y++)
                {
                    var indexOfThisSquare = Puzzle.PieceIndexFromPosition(x, y);
                    var puzzlePiece = Puzzle.PuzzlePieces.First(p => p.Index == indexOfThisSquare);
                    var neighbours = Puzzle.Neighbours(x, y);

                    puzzlePiece.Valid = UserSettings.Instance().PuzzleMatchingStrict ?
                        PieceHasAllValidNeighbours(puzzlePiece.Piece, neighbours) :
                        PieceHasEnoughValidNeighbours(puzzlePiece.Piece, neighbours);
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

            ((PuzzlePieceUi)Controls.Find(piece.Id, true).First()).Piece = piece;
            ValidatePuzzle();
            LoadPieces();

            SaveToDisk?.Invoke();
        }
    }
}