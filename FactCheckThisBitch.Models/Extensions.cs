using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FactCheckThisBitch.Models
{
    public static class Extensions
    {
        public static Type ToPieceContentType(this PieceType pieceType)
        {
            var enumValue = pieceType.ToString();
            Type type = Assembly.GetExecutingAssembly().GetType($"FactCheckThisBitch.Models.{enumValue}");
            return type;
        }

        public static BaseContent ToPieceContent(this PieceType pieceType)
        {
            var enumValue = pieceType.ToString();
            Type type = Assembly.GetExecutingAssembly().GetType($"FactCheckThisBitch.Models.{enumValue}");
            return Activator.CreateInstance(type) as BaseContent;
        }

        public static PieceType ToPieceType(this BaseContent pieceContent)
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
                piece.Content = piece.Type.ToPieceContent();

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
                piece.Content = piece.Type.ToPieceContent();
            }
        }
    }
}
