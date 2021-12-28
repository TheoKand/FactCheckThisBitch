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
        public string Language = "EN";
        public string Thesis;
        public string Conclusion;
        public int Width = 4;
        public int Height = 3;
        public int Duration = 10;
        public string Notes;
        public List<PuzzlePiece> PuzzlePieces= new List<PuzzlePiece>();

        [JsonIgnore]
        public string FullTitle => Title != null ? $"{Title?.ToSanitizedString()}-{Language}" : null;

        [JsonIgnore]
        public string FileName => Title != null ? $"{FullTitle}.json" : null;
    }

    public class PuzzlePiece
    {
        public int Index;
        public int RenderOrder;
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