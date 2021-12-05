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
        public List<string> Images;
        public IContent Content; //depends on type
        public IEnumerable<Reference> References;

        public Piece()
        {
            Id = Guid.NewGuid().ToString();
            Title = "...Title...";
            Keywords = new List<string>();
            Images = new List<string>();
            Content = new Article();
            References = new List<Reference>();
        }
    }
}