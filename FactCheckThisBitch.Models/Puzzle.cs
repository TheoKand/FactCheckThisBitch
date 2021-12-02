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
        public int Width = 4;
        public int Height = 3;
        public PuzzlePiece OnePiece;
        public List<PuzzlePiece> PuzzlePieces;

        public void InitPieces()
        {
            if (PuzzlePieces == null)
            {
                PuzzlePieces = new List<PuzzlePiece>();
                var index = 0;
                for (int i = 0; i < Width * Height; i++)
                {
                    index++;
                    PuzzlePiece puzzlePiece = new PuzzlePiece
                    {
                        Index = index,
                        Piece = new Piece()
                    };
                    PuzzlePieces.Add(puzzlePiece);
                }
            }
        }

        public void Resize()
        {
            InitPieces();
        }
    }

    public class PuzzlePiece
    {
        public int Index;
        public Piece Piece;
        public bool Valid;
    }

    public class PuzzleDisplay
    {
        public Uri Music;
        public string BackColor;
        public string Color;
    }
}