using System;
using System.Collections.Generic;

namespace VideoFromArticle.Models
{
    public class Article
    {
        public string Id { get; set; }
        public int Order { get; set; }


        public string Url { get; set; }
        public string Source { get; set; }
        public string Title { get; set; }
        public DateTime? Published { get; set; }
        public List<string> Images { get; set; }
        public string Narration { get; set; }

        public int DurationInSeconds 
        {
            get
            {
                return 0;
            }
        }

        public Article()
        {
            Id = Guid.NewGuid().ToString();
            Images = new List<string>();
        }
    }

}
