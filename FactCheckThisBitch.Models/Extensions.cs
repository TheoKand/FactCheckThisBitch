using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FactCheckThisBitch.Models
{
    public static class Extensions
    {
        public static IContent ToPieceContentNewInstance(this PieceType pieceType)
        {
            var enumValue = pieceType.ToString();
            Type type = Assembly.GetExecutingAssembly().GetType($"FactCheckThisBitch.Models.{enumValue}");
            return Activator.CreateInstance(type) as IContent;
        }

        public static PieceType ToPieceType(this IContent pieceContent)
        {
            var enumValue = pieceContent.GetType().Name;
            return (PieceType)Enum.Parse(typeof(PieceType), enumValue);
        }

        public static int PieceIndexFromPosition(this Puzzle puzzle, int x, int y)
        {
            int index;

            if (y == 1)
            {
                index = x;
            }
            else
            {
                index = puzzle.Width * (y - 1) + x;
            }

            return index;
        }

        public static void ConvertContentToNewTypeAndKeepMetadata(this Piece piece)
        {
            if (piece.Content != null)
            {
                var oldContent = new
                {
                    piece.Content.Title,
                    piece.Content.Summary,
                    piece.Content.Type,
                    piece.Content.DatePublished,
                    piece.Content.Source,
                    piece.Content.Url,
                    piece.Content.References
                };
                piece.Content = piece.Type.ToPieceContentNewInstance();

                piece.Content.Title = oldContent.Title;
                piece.Content.Summary = oldContent.Summary;
                piece.Content.Type = oldContent.Type;
                piece.Content.DatePublished = oldContent.DatePublished;
                piece.Content.Source = oldContent.Source;
                piece.Content.Url = oldContent.Url;
                piece.Content.References = oldContent.References;
            }
            else
            {
                piece.Content = piece.Type.ToPieceContentNewInstance();
            }
        }

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
