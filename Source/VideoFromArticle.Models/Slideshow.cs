using FackCheckThisBitch.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace VideoFromArticle.Models
{
    public class Slideshow
    {
        public string Id { get; set; }

        public string Title { get; set; }
        public List<SlideshowArticle> Articles { get; set; }



        public DateTime? Created { get; set; }

        [JsonIgnore]
        public string FullTitle => Title != null ? $"{Title?.Sanitize()}" : null;

        [JsonIgnore]
        public string FileName => Title != null ? $"{FullTitle}.json" : null;

        public Slideshow()
        {
            Id = Guid.NewGuid().ToString();
            Articles = new List<SlideshowArticle>();
        }

    }
}
