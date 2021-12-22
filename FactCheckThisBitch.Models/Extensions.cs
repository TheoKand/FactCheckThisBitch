using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using FackCheckThisBitch.Common;

namespace FactCheckThisBitch.Models
{
    public static class Extensions
    {
        public static int PieceIndexFromPosition(this Puzzle puzzle,
            int x,
            int y) =>
            puzzle.Width * (y - 1) + x;

        public static List<PuzzlePiece> Neighbours(this Puzzle puzzle,
            int x,
            int y)
        {
            var neighbours = new List<PuzzlePiece>();
            if (y > 1)
                neighbours.Add(puzzle.PuzzlePieces.First(p => p.Index ==
                                                              puzzle.PieceIndexFromPosition(x,
                                                                  y - 1)));
            if (x > 1)
                neighbours.Add(puzzle.PuzzlePieces.First(p => p.Index ==
                                                              puzzle.PieceIndexFromPosition(x - 1,
                                                                  y)));
            if (x < puzzle.Width)
                neighbours.Add(puzzle.PuzzlePieces.First(p => p.Index ==
                                                              puzzle.PieceIndexFromPosition(x + 1,
                                                                  y)));
            if (y < puzzle.Height)
                neighbours.Add(puzzle.PuzzlePieces.First(p => p.Index ==
                                                              puzzle.PieceIndexFromPosition(x,
                                                                  y + 1)));
            return neighbours;
        }

        public static string ToDescription(this Puzzle puzzle,
            bool includeDescriptions = false,
            bool includeReferenceTitles = false,
            Level level = Level.None)
        {
            StringBuilder result = new StringBuilder();
            result.AppendLine($"--- {puzzle.Title.WrongSpeakToLeetSpeak(level)} ---");
            result.AppendLine($"{puzzle.Thesis.WrongSpeakToLeetSpeak(level)}");
            result.AppendLine();

            foreach (var puzzlePiece in puzzle.PuzzlePieces)
            {
                var piece = puzzlePiece.Piece;

                result.AppendLine($"{puzzlePiece.Index}. {piece.Title.WrongSpeakToLeetSpeak(level)}");
                if (includeDescriptions)
                {
                    result.AppendLine($"{piece.Thesis.WrongSpeakToLeetSpeak(level)}");
                    result.AppendLine();
                }

                foreach (var reference in piece.References)
                {
                    if (includeReferenceTitles)
                    {
                        result.AppendLine($"\t{reference.Title.WrongSpeakToLeetSpeak(level)}");
                    }

                    result.AppendLine($"\t{reference.Url}");
                    result.AppendLine();
                }
            }

            result.AppendLine($"{(puzzle.Language=="EN"?"WHAT DOES IT ALL MEAN???":"ΤΙ ΣΗΜΑΙΝΟΥΝ ΟΛΑ ΑΥΤΑ;;;")}");
            result.AppendLine($"{puzzle.Conclusion.WrongSpeakToLeetSpeak(level)}");
            result.AppendLine();
            result.AppendLine($"#FactCheckThisBitch");

            return result.ToString();
        }
    }
}