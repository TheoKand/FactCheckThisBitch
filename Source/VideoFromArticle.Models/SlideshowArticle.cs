using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using FackCheckThisBitch.Common;

namespace VideoFromArticle.Models
{
    public class SlideshowArticle
    {
        public string Id { get; set; }
        public int Order { get; set; }


        public string Url { get; set; }
        public string Source { get; set; }
        public string Title { get; set; }
        public DateTime? Published { get; set; }
        public List<ArticleImage> Images { get; set; }
        public string Narration { get; set; }

        public DateTime? Fetch { get; set; }

        [JsonIgnore]
        public List<ArticleImage> RecycledImages { get; set; }

        public double DurationInSeconds { get; set; }

        public override string ToString()
        {
            return $"{Id} {Title.Limit(50)}";
        }

        public SlideshowArticle()
        {
            Id = Guid.NewGuid().ToString();
            Title = $"New Article With Id {Id}";
            Images = new List<ArticleImage>();
            RecycledImages = new List<ArticleImage>();
        }
    }

    public class ArticleImage
    {
        public string Url { get; set; }
        public long Filesize { get; set; }
        public string Filename { get; set; }

        public ArticleImage(string url)
        {
            Url = url;
        }
    }

}
