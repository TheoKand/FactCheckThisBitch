using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;

namespace FactCheckThisBitch.Models
{
    public class Reference
    {
        public ReferenceType Type;
        public string Title { get; set; }
        public string Description { get; set; }
        public string Source { get; set; }
        public string Url { get; set; }
        public List<string> Images { get; set; }
        public DateTime? DatePublished { get; set; }
        public string Author { get; set; }
        public string OriginalSource { get; set; }

        public Reference()
        {
            Images = new List<string>();
        }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum ReferenceType
    {
        [EnumMember(Value = "")] Unknown,
        [EnumMember(Value = "Article")] Article,
        [EnumMember(Value = "BookExcerpt")] BookExcerpt,
        [EnumMember(Value = "WebVideo")] WebVideo,
    }

}
