using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;

namespace FactCheckThisBitch.Models
{
    public class Piece
    {
        private string Title;
        private string Thesis;
        private string[] Keywords;
        public string[] Images;

        private PieceType Type;
        public BaseContent Content;
        public PieceDisplay Display;
    }

    public class PieceDisplay
    {
        public Uri Image;
        public Uri Thumbnail;
    }

    public enum PieceType
    {
        Argument,
        Article,
        BookExcerpt,
        Interview,
        Podcast,
        Study,
        Statistic,
        DictionaryDefinition,
        WebVideo,
        Documentary,
        WebSearch,
        NewsPaperArticle,
        Comparisson,
    }



}
