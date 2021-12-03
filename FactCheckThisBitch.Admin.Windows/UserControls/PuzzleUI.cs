using FactCheckThisBitch.Admin.Windows.Forms;
using FactCheckThisBitch.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using FackCheckThisBitch.Common;

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
                    piece.Keywords.HaveAtLeastOneCommonKeyword(connectedPiece.Piece.Keywords);

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
                    piece.Keywords.HaveAtLeastOneCommonKeyword(connectedPiece.Piece.Keywords);

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

                    var neighbours = Puzzle.Neighbours(x, y);

                    puzzlePiece.Valid = UserSettings.Instance().PuzzleMatchingStrict
                        ? PieceHasAllValidNeighbours(puzzlePiece.Piece, neighbours)
                        : PieceHasEnoughValidNeighbours(puzzlePiece.Piece, neighbours);

                    var puzzlePieceUi = (PuzzlePieceUi) Controls.Find(puzzlePiece.Piece.Id, true).First();

                    var leftNeighbour = neighbours.FirstOrDefault(n => n.X == x - 1 && n.Y == y);
                    if (leftNeighbour != null)
                    {
                        bool connectedToLeftNeighbour = leftNeighbour.Piece.Keywords.HaveAtLeastOneCommonKeyword(
                            puzzlePiece.Piece.Keywords);
                        puzzlePieceUi.ConnectedLeft = connectedToLeftNeighbour;
                    }

                    var rightNeighbour = neighbours.FirstOrDefault(n => n.X == x + 1 && n.Y == y);
                    if (rightNeighbour != null)
                    {
                        bool connectedToRightNeighbour = rightNeighbour.Piece.Keywords.HaveAtLeastOneCommonKeyword(
                            puzzlePiece.Piece.Keywords);
                        puzzlePieceUi.ConnectedRight = connectedToRightNeighbour;
                    }

                    var topNeighbour = neighbours.FirstOrDefault(n => n.X == x && n.Y == y - 1);
                    if (topNeighbour != null)
                    {
                        bool connectedTopNeighbour = topNeighbour.Piece.Keywords.HaveAtLeastOneCommonKeyword(
                            puzzlePiece.Piece.Keywords);
                        puzzlePieceUi.ConnectedTop = connectedTopNeighbour;
                    }

                    var bottomNeighbour = neighbours.FirstOrDefault(n => n.X == x && n.Y == y + 1);
                    if (bottomNeighbour != null)
                    {
                        bool connectedBottomNeighbour = bottomNeighbour.Piece.Keywords.HaveAtLeastOneCommonKeyword(
                            puzzlePiece.Piece.Keywords);
                        puzzlePieceUi.ConnectedBottom = connectedBottomNeighbour;
                    }
                }
            }
        }

        private void OnPieceDragDrop(string firstPieceId, string secondPieceId)
        {
            if (firstPieceId == secondPieceId) return;

            var firstPuzzlePiece = _puzzle.PuzzlePieces.First(p => p.Piece.Id == firstPieceId);
            var secondPuzzlePiece = _puzzle.PuzzlePieces.First(p => p.Piece.Id == secondPieceId);

            (secondPuzzlePiece.Index, firstPuzzlePiece.Index) = (firstPuzzlePiece.Index, secondPuzzlePiece.Index);
            (secondPuzzlePiece.X, firstPuzzlePiece.X) = (firstPuzzlePiece.X, secondPuzzlePiece.X);
            (secondPuzzlePiece.Y, firstPuzzlePiece.Y) = (firstPuzzlePiece.Y, secondPuzzlePiece.Y);

            Puzzle.ReorderPieces();

            //var firstPieceUi = ((PuzzlePieceUi) Controls.Find(firstPieceId, true).First());
            //var secondPieceUi = ((PuzzlePieceUi) Controls.Find(secondPieceId, true).First());

            //(secondPieceUi.Location, firstPieceUi.Location) = (firstPieceUi.Location, secondPieceUi.Location);

            
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