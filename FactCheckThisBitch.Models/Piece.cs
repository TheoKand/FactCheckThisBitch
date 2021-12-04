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
        public PieceType Type;

        public Piece()
        {
            Id = Guid.NewGuid().ToString();
            Title = "...Title...";
            Type = PieceType.Article;
            Content = new Article();
        }
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

        [EnumMember(Value = "NewsPaperArticle")]
        NewsPaperArticle,
        [EnumMember(Value = "MyArgument")] MyArgument,
    }
}