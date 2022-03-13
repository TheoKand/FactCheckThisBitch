﻿using System;
using System.Collections.Generic;
using FackCheckThisBitch.Common;

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
        public List<ArticleImage> Images { get; set; }
        public string Narration { get; set; }

        public DateTime? Fetch { get; set; }

        public int DurationInSeconds 
        {
            get
            {
                return 0;
            }
        }

        public override string ToString()
        {
            return $"{Title.Limit(50)}\tImages: {Images.Count}\tNarration: {DurationInSeconds} sec ";
        }

        public Article()
        {
            Id = Guid.NewGuid().ToString();
            Images = new List<ArticleImage>();
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
