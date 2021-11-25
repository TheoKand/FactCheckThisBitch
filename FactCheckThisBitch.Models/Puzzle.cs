using System;
using System.Collections.Generic;
using System.Text;

namespace FactCheckThisBitch.Models
{
    public class Puzzle
    {
        public string Title;
        public string Thesis;
        public string Conclusion;
        public int Width = 3;
        public int Height = 3;

        public PuzzlePiece[] Pieces;
        public PuzzlePiece[] PiecesThatDontFit;
    }

    public class PuzzlePiece
    {
        public int Number;
        public Piece Piece;
        public Piece[] ConnectingPieces;
    }

    public class PuzzleDisplay
    {
        public Uri Music;
        public string BackColor;
        public string Color;
    }
}
