using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace FactCheckThisBitch.Models
{
    public class Reference
    {
        public string Id { get; set; }
        public ReferenceType Type;
        public string Title { get; set; }
        public string Description { get; set; }
        public double NarrationDuration { get; set; }
        public string Source { get; set; }
        public string Url { get; set; }
        public int Duration { get; set; }
        public List<string> Images { get; set; }
        public List<ImageEdit> ImageEdits { get; set; }
        public DateTime? DatePublished { get; set; }
        public string Author { get; set; }
        public string OriginalSource { get; set; }

        public Reference()
        {
            Id = Guid.NewGuid().ToString();
            Images = new List<string>();
            ImageEdits = new List<ImageEdit>();
            Duration = 8;
        }
    }

    public class ImageEdit
    {
        public string Image;
        public List<Rectangle> BlurryAreas = new List<Rectangle>();
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