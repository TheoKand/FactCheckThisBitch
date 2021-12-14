using FactCheckThisBitch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FactCheckThisBitch.Render
{
    public static class Extensions
    {
        public static string GetKeywordsText(this Piece piece)
        {
            var result = string.Join(Environment.NewLine, piece.Keywords.Select(k => $"• {k.Trim()}"));
            return result;
        }

        public static dynamic GetPieceCoordinates(this Puzzle puzzle, int pieceIndex)
        {
            switch (pieceIndex)
            {
                case 1:
                    return new
                    {
                        X = 17, Y = 11, KeywordsBox = new
                        {
                            Left = 0,
                            Top = 0,
                            Right = 250,
                            Bottom = 157
                        }
                    };
                case 2:
                    return new {X = 280, Y = 11, KeywordsBox = new
                        {
                            Left = 0,
                            Top = 0,
                            Right = 250,
                            Bottom = 157
                        }};
                case 3:
                    return new {X = 449, Y = 11, KeywordsBox = new
                    {
                        Left = 95,
                        Top = 0,
                        Right = 344,
                        Bottom = 156
                    }};
                case 4:
                    return new {X = 17, Y = 182, KeywordsBox = new
                    {
                        Left = 0,
                        Top = 96,
                        Right = 250,
                        Bottom = 173
                    }};
                case 5:
                    return new {X = 186, Y = 182, KeywordsBox = new
                    {
                        Left = 99,
                        Top = 99,
                        Right = 344,
                        Bottom = 346
                    }};
                case 6:
                    return new {X = 544, Y = 182, KeywordsBox = new
                    {
                        Left = 1,
                        Top = 99,
                        Right = 248,
                        Bottom = 172
                    }};
                case 7:
                    return new {X = 18, Y = 543, KeywordsBox = new
                    {
                        Left = 2,
                        Top = 98,
                        Right = 246,
                        Bottom = 250
                    }};
                case 8:
                    return new {X = 280, Y = 543, KeywordsBox = new
                    {
                        Left = 3,
                        Top = 183,
                        Right = 248,
                        Bottom = 251
                    }};
                case 9:
                    return new {X = 449, Y = 543, KeywordsBox = new
                    {
                        Left = 97,
                        Top = 97,
                        Right = 342,
                        Bottom = 248
                    }};
                default:
                    return new {X = 50, Y = 50, KeywordsBox = new
                    {
                        Left = 0,
                        Top = 0,
                        Right = 0,
                        Bottom = 0
                    }};
            }
        }
    }
}