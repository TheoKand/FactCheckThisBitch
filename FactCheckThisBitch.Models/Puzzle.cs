using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FackCheckThisBitch.Common;
using Newtonsoft.Json;

namespace FactCheckThisBitch.Models
{
    public class Puzzle
    {
        public string Title;
        public string Thesis;
        public string Conclusion;
        public int Width = 4;
        public int Height = 3;
        public List<PuzzlePiece> PuzzlePieces= new List<PuzzlePiece>();

        [JsonIgnore]
        public string FileName => Title != null ? $"{Title?.ToSanitizedString()}.json" : null;
    }

    public class PuzzlePiece
    {
        public int Index;
        public int X;
        public int Y;
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