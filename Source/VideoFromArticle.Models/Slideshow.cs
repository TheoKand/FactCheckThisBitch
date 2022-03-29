using FackCheckThisBitch.Common;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using System.Text.Json.Serialization;

namespace VideoFromArticle.Models
{
    public class Slideshow
    {
        public string Id { get; set; }

        public string Title { get; set; }
        public List<Article> Articles { get; set; }



        public DateTime? Created { get; set; }

        [JsonIgnore]
        public string SanitizedTitle => Title != null ? $"{Title?.Sanitize()}" : null;

        public Slideshow()
        {
            Id = Guid.NewGuid().ToString();
            Articles = new List<Article>();
        }

    }
}
