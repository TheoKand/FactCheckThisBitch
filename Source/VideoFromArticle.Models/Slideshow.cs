using System;
using System.Collections.Generic;
using System.Text;

namespace VideoFromArticle.Models
{
    public class Slideshow
    {
        public string Id { get; set; }

        public string Title { get; set; }
        public List<Article> Articles { get; set; }

        public DateTime? Created { get; set; }

        public Slideshow()
        {
            Id = Guid.NewGuid().ToString();
            Articles = new List<Article>();
        }

    }
}
