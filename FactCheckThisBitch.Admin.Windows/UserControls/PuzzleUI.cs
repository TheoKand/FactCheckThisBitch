using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Forms;
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
                _puzzle = value;
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
            Controls.Clear();

            if (_puzzle == null) return;

            const int leftMargin = 10;
            const int topMargin = 5;
            const int pieceWidth = 176;
            const int pieceHeight = 122;

            for (int x = 1; x <= Puzzle.Width; x++)
            {
                for (int y = 1; y <= Puzzle.Height; y++)
                {
                    var indexOfThisSquare = Puzzle.PieceIndexFromPosition(x, y);

                    var piece = Puzzle.PuzzlePieces?.FirstOrDefault(p => p.Index == indexOfThisSquare)?.Piece;
                    if (piece == null)
                    {
                        piece = new Piece();
                    }
                    var puzzlePieceX = leftMargin + (x - 1) * (pieceWidth + leftMargin);
                    var puzzlePieceY = topMargin + (y - 1) * (pieceHeight + topMargin);

                    PuzzlePieceUI pieceUi = new PuzzlePieceUI()
                    {
                        Name = piece.Id,
                        Piece = piece,
                        Left = puzzlePieceX,
                        Top = puzzlePieceY,
                    };
                    pieceUi.OnClick = () => OnPieceClicked(piece);

                    Controls.Add(pieceUi);

                }
            }
        }

        private void OnPieceClicked(Piece piece)
        {
            FrmPiece pieceForm = new FrmPiece(piece);
            var result = pieceForm.ShowDialog();
            if (result != DialogResult.OK) return;

            (Controls.Find(piece.Id, true).First() as PuzzlePieceUI).Piece = piece;
        }
    }
}
