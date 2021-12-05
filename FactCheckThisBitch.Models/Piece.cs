using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace FactCheckThisBitch.Models
{
    public class Piece
    {
        public string Id;
        public string Title;
        public string Thesis;
        public List<string> Keywords;
        public IContent Content; //depends on type
        public List<Reference> References;

        public Piece()
        {
            Id = Guid.NewGuid().ToString();
            Title = "...Title...";
            Keywords = new List<string>();
            Content = new Article();
            References = new List<Reference>();
        }

        public void AddDummyReference()
        {
            References.Add(
                new Reference()
                {
                    Type = ReferenceType.Article,
                    Title = "title",
                    Description = "desc",
                    Source = "source",
                    Url = "http://theok.com",
                    Images = new List<string>(),
                    DatePublished = DateTime.Today,
                    Author = "Author"
                });
        }
    }
}