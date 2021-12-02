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

                LoadPieces();
                DecoratePuzzle();
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

            const int padding = 50;
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
                        puzzlePiece = new PuzzlePiece
                        {
                            Index = indexOfThisSquare,
                            Piece = piece
                        };
                        _puzzle.PuzzlePieces.Add(puzzlePiece);
                    }

                    puzzlePiece.X = x;
                    puzzlePiece.Y = y;

                    #region create puzzle piece ui and add to form

                    var puzzlePieceX = 5 + (x - 1) * (pieceWidth + padding);
                    var puzzlePieceY = 5 + (y - 1) * (pieceHeight + padding);

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

                    #endregion
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
                maxInvalidNeighbours = 2;
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

        private void DecoratePuzzle()
        {
            for (int x = 1; x <= Puzzle.Width; x++)
            {
                for (int y = 1; y <= Puzzle.Height; y++)
                {
                    var indexOfThisSquare = Puzzle.PieceIndexFromPosition(x, y);
                    var puzzlePiece = Puzzle.PuzzlePieces.First(p => p.Index == indexOfThisSquare);
                    puzzlePiece.X = x;
                    puzzlePiece.Y = y;

                    var neighbours = Puzzle.Neighbours(x, y);

                    puzzlePiece.Valid = UserSettings.Instance().PuzzleMatchingStrict
                        ? PieceHasAllValidNeighbours(puzzlePiece.Piece, neighbours)
                        : PieceHasEnoughValidNeighbours(puzzlePiece.Piece, neighbours);

                    var puzzlePieceUi = (PuzzlePieceUi) Controls.Find(puzzlePiece.Piece.Id, true).First();

                    var leftNeighbour = neighbours.FirstOrDefault(n => n.X == x - 1 && n.Y == y);
                    if (leftNeighbour != null)
                    {
                        bool connectedToLeftNeighbour = HaveAtLeastOneCommonKeyword(leftNeighbour?.Piece.Keywords,
                            puzzlePiece.Piece.Keywords);
                        puzzlePieceUi.ConnectedLeft = connectedToLeftNeighbour;
                    }

                    var rightNeighbour = neighbours.FirstOrDefault(n => n.X == x + 1 && n.Y == y);
                    if (rightNeighbour != null)
                    {
                        bool connectedToRightNeighbour = HaveAtLeastOneCommonKeyword(rightNeighbour?.Piece.Keywords,
                            puzzlePiece.Piece.Keywords);
                        puzzlePieceUi.ConnectedRight = connectedToRightNeighbour;
                    }

                    var topNeighbour = neighbours.FirstOrDefault(n => n.X == x && n.Y == y - 1);
                    if (topNeighbour != null)
                    {
                        bool connectedTopNeighbour = HaveAtLeastOneCommonKeyword(topNeighbour?.Piece.Keywords,
                            puzzlePiece.Piece.Keywords);
                        puzzlePieceUi.ConnectedTop = connectedTopNeighbour;
                    }

                    var bottomNeighbour = neighbours.FirstOrDefault(n => n.X == x && n.Y == y + 1);
                    if (bottomNeighbour != null)
                    {
                        bool connectedBottomNeighbour = HaveAtLeastOneCommonKeyword(bottomNeighbour?.Piece.Keywords,
                            puzzlePiece.Piece.Keywords);
                        puzzlePieceUi.ConnectedBottom = connectedBottomNeighbour;
                    }
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

            Puzzle.ReorderPieces();
            LoadPieces();
            DecoratePuzzle();
        }

        private void OnPieceClicked(Piece piece)
        {
            FrmPiece pieceForm = new FrmPiece(piece);
            var result = pieceForm.ShowDialog();
            if (result != DialogResult.OK) return;

            ((PuzzlePieceUi) Controls.Find(piece.Id, true).First()).Piece = piece;

            LoadPieces();
            DecoratePuzzle();

            SaveToDisk?.Invoke();
        }
    }
}