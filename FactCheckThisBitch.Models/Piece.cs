using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;
using FackCheckThisBitch.Common;

namespace FactCheckThisBitch.Models
{
    public class Piece
    {
        public string Id;
        public string Title;
        public string Thesis;
        public string[] Keywords;
        public string[] Images;
        public BaseContent Content; //depends on type
        public PieceDisplay Display;

        private PieceType _type;
        public PieceType Type
        {
            get => _type;
            set
            {
                _type = value;
                //TODO: convert content type? Copy values with property copier?
                Content = _type.ToPieceContent();
            }
        }

        public Piece()
        {
            Id = Guid.NewGuid().ToString();
            Title = "Enter your thesis title here. What do you want to show with this piece.";
            Thesis = "Elaborate on your entire thesis here";
            Type = PieceType.Article;
            Keywords = new string[] { };
            Images = new string[] { };
            Content = new Article();
            Display = new PieceDisplay();
        }


    }

    public class PieceDisplay
    {
        public Uri Image;
        public Uri Thumbnail;
    }

    public enum PieceType
    {
        Article,
        BookExcerpt,
        Interview,
        Podcast,
        Study,
        Statistic,
        Definition,
        WebVideo,
        Documentary,
        WebSearch,
        NewsPaperArticle,
        Comparisson,
        MyArgument,
    }



}
