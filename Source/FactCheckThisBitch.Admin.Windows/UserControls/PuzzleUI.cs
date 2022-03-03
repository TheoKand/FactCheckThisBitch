using FactCheckThisBitch.Admin.Windows.Forms;
using FactCheckThisBitch.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using FackCheckThisBitch.Common;
using Newtonsoft.Json;

namespace FactCheckThisBitch.Admin.Windows.UserControls
{
    public partial class PuzzleUi : UserControl
    {
        private Puzzle _puzzle;
        public Action SaveToFile;
        public Action OnChanged;

        private string _selectedPieceId;

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
                    var puzzlePiece = Puzzle.PuzzlePieces?.FirstOrDefault(p => p.Index == indexOfThisSquare);
                    var piece = puzzlePiece?.Piece;
                    if (piece == null)
                    {
                        piece = new Piece();
                        puzzlePiece = new PuzzlePiece { Index = indexOfThisSquare, Piece = piece };
                        _puzzle.PuzzlePieces.Add(puzzlePiece);
                    }

                    puzzlePiece.X = x;
                    puzzlePiece.Y = y;

                    #region create puzzle piece ui and add to form

                    var puzzlePieceX = 5 + (x - 1) * (pieceWidth + padding);
                    var puzzlePieceY = 5 + (y - 1) * (pieceHeight + padding);

                    PuzzlePieceUi puzzlePieceUi = new PuzzlePieceUi()
                    {
                        PuzzlePiece = puzzlePiece,
                        Name = piece.Id,
                        Left = puzzlePieceX,
                        Top = puzzlePieceY,
                    };

                    puzzlePieceUi.OnClick = () => OnPieceClicked(piece);
                    puzzlePieceUi.OnDragDrop = OnPieceDragDrop;
                    puzzlePieceUi.OnChanged = OnChanged;

                    Controls.Add(puzzlePieceUi);

                    #endregion
                }
            }

            this.ResumeLayout();
        }

        private bool PieceHasAllValidNeighbours(Piece piece, List<PuzzlePiece> neighbours)
        {
            if (!piece.Keywords.Any() || piece.Keywords.All(string.IsNullOrWhiteSpace))
            {
                return false;
            }

            bool atLeastOneConnectedPieceWithoutCommonKeywords = false;
            foreach (var connectedPiece in neighbours)
            {
                bool haveCommonKeyWords = piece.Keywords.HaveAtLeastOneCommonKeyword(connectedPiece.Piece.Keywords);

                if (!haveCommonKeyWords)
                {
                    atLeastOneConnectedPieceWithoutCommonKeywords = true;
                }
            }

            return !atLeastOneConnectedPieceWithoutCommonKeywords;
        }

        private bool PieceHasEnoughValidNeighbours(Piece piece, List<PuzzlePiece> neighbours)
        {
            if (!piece.Keywords.Any()) return false;

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
                bool haveCommonKeyWords = piece.Keywords.HaveAtLeastOneCommonKeyword(connectedPiece.Piece.Keywords);

                if (!haveCommonKeyWords)
                {
                    invalidNeighbours++;
                }
            }

            return invalidNeighbours <= maxInvalidNeighbours;
        }

        private void SelectPiece(string pieceId, bool selected)
        {
            if (pieceId == null) return;
            ((PuzzlePieceUi)Controls.Find(pieceId, true).First()).BorderStyle = selected ? BorderStyle.FixedSingle : BorderStyle.None;
        }

        private void DecoratePuzzle()
        {
            for (int x = 1; x <= Puzzle.Width; x++)
            {
                for (int y = 1; y <= Puzzle.Height; y++)
                {

                    var indexOfThisSquare = Puzzle.PieceIndexFromPosition(x, y);
                    var puzzlePiece = Puzzle.PuzzlePieces.First(p => p.Index == indexOfThisSquare);
                    var puzzlePieceUi = (PuzzlePieceUi)Controls.Find(puzzlePiece.Piece.Id, true).First();

                    puzzlePieceUi.BorderStyle = puzzlePiece.Piece.Id == _selectedPieceId
                        ? BorderStyle.FixedSingle
                        : BorderStyle.None;

                    #region find neghbours and establish if the piece connects or not

                    var neighbours = Puzzle.Neighbours(x, y);

                    puzzlePiece.Valid = UserSettings.Instance().PuzzleMatchingStrict
                        ? PieceHasAllValidNeighbours(puzzlePiece.Piece, neighbours)
                        : PieceHasEnoughValidNeighbours(puzzlePiece.Piece, neighbours);

                    puzzlePieceUi.BackColor = puzzlePiece != null && puzzlePiece.Valid
                        ? Color.LightGreen
                        : Color.FromArgb(255, 192, 192);

                    #endregion

                    #region set dots around the piece

                    var leftNeighbour = neighbours.FirstOrDefault(n => n.X == x - 1 && n.Y == y);
                    if (leftNeighbour != null)
                    {
                        bool connectedToLeftNeighbour =
                            leftNeighbour.Piece.Keywords.HaveAtLeastOneCommonKeyword(puzzlePiece.Piece.Keywords);
                        puzzlePieceUi.ConnectedLeft = connectedToLeftNeighbour;
                    }

                    var rightNeighbour = neighbours.FirstOrDefault(n => n.X == x + 1 && n.Y == y);
                    if (rightNeighbour != null)
                    {
                        bool connectedToRightNeighbour =
                            rightNeighbour.Piece.Keywords.HaveAtLeastOneCommonKeyword(puzzlePiece.Piece.Keywords);
                        puzzlePieceUi.ConnectedRight = connectedToRightNeighbour;
                    }

                    var topNeighbour = neighbours.FirstOrDefault(n => n.X == x && n.Y == y - 1);
                    if (topNeighbour != null)
                    {
                        bool connectedTopNeighbour =
                            topNeighbour.Piece.Keywords.HaveAtLeastOneCommonKeyword(puzzlePiece.Piece.Keywords);
                        puzzlePieceUi.ConnectedTop = connectedTopNeighbour;
                    }

                    var bottomNeighbour = neighbours.FirstOrDefault(n => n.X == x && n.Y == y + 1);
                    if (bottomNeighbour != null)
                    {
                        bool connectedBottomNeighbour =
                            bottomNeighbour.Piece.Keywords.HaveAtLeastOneCommonKeyword(puzzlePiece.Piece.Keywords);
                        puzzlePieceUi.ConnectedBottom = connectedBottomNeighbour;
                    }

                    #endregion
                }
            }
        }

        private void OnPieceDragDrop(int draggedPieceIndex, int destinationPieceIndex)
        {
            if (draggedPieceIndex == destinationPieceIndex) return;
            var dragged = _puzzle.PuzzlePieces.First(p => p.Index == draggedPieceIndex);
            var destination = _puzzle.PuzzlePieces.First(p => p.Index == destinationPieceIndex);

            //swap puzzle pieces within the array
            _puzzle.PuzzlePieces =
                (List<PuzzlePiece>)_puzzle.PuzzlePieces.Swap<PuzzlePiece>(draggedPieceIndex - 1, destinationPieceIndex - 1);

            //also change the position-related properties
            (destination.Index, dragged.Index) = (dragged.Index, destination.Index);
            (destination.X, dragged.X) = (dragged.X, destination.X);
            (destination.Y, dragged.Y) = (dragged.Y, destination.Y);

            //swap in the UI
            var draggedUi = ((PuzzlePieceUi)Controls.Find(dragged.Piece.Id, true).First());
            var destinationUi = ((PuzzlePieceUi)Controls.Find(destination.Piece.Id, true).First());
            (destinationUi.Location, draggedUi.Location) = (draggedUi.Location, destinationUi.Location);

            OnChanged?.Invoke();
            DecoratePuzzle();
        }

        private void OnPieceClicked(Piece piece)
        {
            SelectPiece(_selectedPieceId, false);
            _selectedPieceId = piece.Id;
            SelectPiece(_selectedPieceId, true);

            using (FrmPiece pieceForm = new FrmPiece(piece))
            {
                var pieceBefore = JsonConvert.SerializeObject(piece, StaticSettings.JsonSerializerSettings);
                pieceForm.OnMoveReferenceToOtherPiece = referenceId => MoveReferenceToOtherPiece(piece, referenceId);
                pieceForm.OnSave = SaveToFile;
                var result = pieceForm.ShowDialog();
                if (result != DialogResult.OK) return;

                var pieceAfter = JsonConvert.SerializeObject(piece, StaticSettings.JsonSerializerSettings);

                if (pieceBefore != pieceAfter)
                {
                    pieceBefore = null;
                    OnChanged?.Invoke();
                    LoadPieces();
                    DecoratePuzzle();
                }
            }

    }

    private bool MoveReferenceToOtherPiece(Piece previousPiece, string referenceId)
    {
        FrmSelectPiece selectPieceForm = new FrmSelectPiece(_puzzle.PuzzlePieces.Select(pp => pp.Piece.Title).ToList());
        var result = selectPieceForm.ShowDialog();
        if (result != DialogResult.OK) return false;

        var newPiece = _puzzle.PuzzlePieces.First(pp => pp.Piece.Title == selectPieceForm.SelectedPieceTitle).Piece;
        var reference = previousPiece.References.First(r => r.Id == referenceId);
        previousPiece.References.Remove(reference);
        newPiece.References.Add(reference);

        LoadPieces();
        DecoratePuzzle();

        return true;
    }
}
}