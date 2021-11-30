using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Runtime.Serialization;

namespace FactCheckThisBitch.Models
{
    public class Piece
    {
        public string Id;
        public string Title;
        public string Thesis;
        public string[] Keywords;
        public string[] Images;
        public IContent Content; //depends on type
        public PieceDisplay Display;
        private PieceType _type;

        public PieceType Type
        {
            get => _type;
            set
            {

                if (_type == value) return;

                _type = value;
                this.ConvertContentToNewTypeAndKeepMetadata();
            }
        }

        public Piece()
        {
            Id = Guid.NewGuid().ToString();
            Title = "...Title...";
            Thesis = "...Thesis...";
            Keywords = new string[] { "First", "Second" };
            Images = new string[] { };
            
            Type = PieceType.Article;
            Content = new Article();
        }
    }

    public class PieceDisplay
    {
        public string Image;
        public string Thumbnail;
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum PieceType
    {
        [EnumMember(Value = "")] Unknown,
        [EnumMember(Value = "Article")] Article,
        [EnumMember(Value = "BookExcerpt")] BookExcerpt,
        [EnumMember(Value = "Interview")] Interview,
        [EnumMember(Value = "Podcast")] Podcast,
        [EnumMember(Value = "Study")] Study,
        [EnumMember(Value = "Statistic")] Statistic,
        [EnumMember(Value = "Definition")] Definition,
        [EnumMember(Value = "WebVideo")] WebVideo,
        [EnumMember(Value = "Documentary")] Documentary,
        [EnumMember(Value = "WebSearch")] WebSearch,

        [EnumMember(Value = "NewsPaperArticle")] NewsPaperArticle,
        [EnumMember(Value = "MyArgument")] MyArgument,
    }
}