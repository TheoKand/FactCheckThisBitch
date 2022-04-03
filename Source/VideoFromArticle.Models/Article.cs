using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using FackCheckThisBitch.Common;

namespace VideoFromArticle.Models
{
    public class Article
    {
        public string Id { get; set; }
        public string SlideshowFolder { get; set; }
        public bool? NarrationPerImage { get; set; }
        public string Url { get; set; }
        public string Source { get; set; }
        public string Title { get; set; }
        public DateTime? Published { get; set; }
        public List<ArticleImage> Images { get; set; }
        public string Narration { get; set; }
        public double DurationInSeconds { get; set; }

        public override string ToString()
        {
            return
                $"{Id.Limit(8).PadRight(10)}{Source?.Limit(12).PadRight(14)}{Published?.ToString("dd/MM/yy").PadRight(10)}{Title?.Limit(75).PadRight(80)}{Narration?.Limit(25).PadRight(27)}";
        }

        public Article()
        {
            Id = Guid.NewGuid().ToString();
            Title = $"New Article With Id {Id}";
            Images = new List<ArticleImage>();
            NarrationPerImage = true;
        }
    }

    public class ArticleImage
    {
        public string Url { get; set; }
        public long Filesize { get; set; }
        public string Filename { get; set; }
        public string Caption { get; set; }
        public string Narration { get; set; }
        public double SlideDurationInSeconds { get; set; }
        public double AudioDuration { get; set; }
        public bool Group { get; set; }
        public bool TypewriterAnimation { get; set; }

        public ArticleImage(string url)
        {
            Narration = "";
            Url = url;
        }
    }
}