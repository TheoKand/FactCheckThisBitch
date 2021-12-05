using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;

namespace FactCheckThisBitch.Models
{
    public static class Extensions
    {

        public static int PieceIndexFromPosition(this Puzzle puzzle, int x, int y) => puzzle.Width * (y - 1) + x;

        public static List<PuzzlePiece> Neighbours(this Puzzle puzzle, int x, int y)
        {
            var neighbours = new List<PuzzlePiece>();
            if (y > 1)
                neighbours.Add(puzzle.PuzzlePieces.First(p =>
                    p.Index == puzzle.PieceIndexFromPosition(x, y - 1)));
            if (x > 1)
                neighbours.Add(puzzle.PuzzlePieces.First(p =>
                    p.Index == puzzle.PieceIndexFromPosition(x - 1, y)));
            if (x < puzzle.Width)
                neighbours.Add(puzzle.PuzzlePieces.First(p =>
                    p.Index == puzzle.PieceIndexFromPosition(x + 1, y)));
            if (y < puzzle.Height)
                neighbours.Add(puzzle.PuzzlePieces.First(p =>
                    p.Index == puzzle.PieceIndexFromPosition(x, y + 1)));
            return neighbours;
        }


    }
}
