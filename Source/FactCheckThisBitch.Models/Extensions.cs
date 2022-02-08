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
            bool includePieceTitles = false,
            bool includeReferenceDescriptions = false,
            Level level = Level.None)
        {
            StringBuilder result = new StringBuilder();
            result.AppendLine($"--- {puzzle.Title.WrongSpeakToLeetSpeak(level)} ---");
            result.AppendLine($"{puzzle.Thesis.WrongSpeakToLeetSpeak(level)}");
            

            foreach (var puzzlePiece in puzzle.PuzzlePieces)
            {
                var piece = puzzlePiece.Piece;

                result.AppendLine();
                
                if (includePieceTitles)
                {
                    result.AppendLine($"{puzzlePiece.Index}. {piece.Title.WrongSpeakToLeetSpeak(level)}");
                    result.AppendLine();
                }

                foreach (var reference in piece.References)
                {
                    if (includeReferenceDescriptions && reference.Description!=null)
                    {
                        result.AppendLine($"{reference.Description.WrongSpeakToLeetSpeak(level)}");
                    }

                    result.AppendLine($"{reference.Url}");
                }
            }

            result.AppendLine($"{(puzzle.Language=="EN"?"WHAT DOES IT ALL MEAN???":"ΤΙ ΣΗΜΑΙΝΟΥΝ ΟΛΑ ΑΥΤΑ;;;")}");
            result.AppendLine($"{puzzle.Conclusion.WrongSpeakToLeetSpeak(level)}");
            result.AppendLine();
            result.AppendLine($"#FactCheckThisBitch");

            return result.ToString();
        }

        public static double ToNarrationDuration(this Reference reference)
        {
            if (!reference.Description.IsEmpty())
            {
                return reference.NarrationDuration == 0
                    ? reference.Description.CalculateNarrationDuration()
                    : reference.NarrationDuration;
            }

            return 0;
        }

        public static TimeSpan ToPositionInPowerpointVideo(this Piece piece,
            List<KeyValuePair<TimeSpan, Reference>> timeline)
        {
            var narration = timeline.First(_ => _.Value.Id == piece.References.First().Id);
            return narration.Key;
        }
    }
}