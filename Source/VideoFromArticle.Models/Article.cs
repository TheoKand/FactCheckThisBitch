using FackCheckThisBitch.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace VideoFromArticle.Models
{
    public class Article
    {
        public string Id { get; set; }
        public string SlideshowFolder { get; set; }
        public string Url { get; set; }
        public string Source { get; set; }
        public string Title { get; set; }
        public DateTime? Published { get; set; }
        public List<ArticleImage> Images { get; set; }

        public double SlideDurationInSeconds()
        {
            return Images.Sum(i => i.SlideDurationInSeconds);
        }

        public override string ToString()
        {
            return
                $"{Id.Limit(8),-10}{Source?.Limit(12),-14}{Published?.ToString("dd/MM/yy"),-10}{Title?.Limit(75),-80}";
        }

        public Article()
        {
            Id = Guid.NewGuid().ToString();
            Title = $"New Article With Id {Id}";
            Images = new List<ArticleImage>();
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

        public ArticleImage(string url)
        {
            Narration = "";
            Url = url;
        }
    }
}