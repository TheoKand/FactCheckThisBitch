using System;
using System.Collections.Generic;
using System.Text;

namespace FactCheckThisBitch.Models
{
    public interface IContent
    {
        public string Title { get; set; }
        public string Summary { get; set; }
        public PieceType Type { get; set; }
        public string Source { get; set; }
        public string Url { get; set; }
        public string[] References { get; set; }
        public DateTime? DatePublished { get; set; }

    }

    public class Article : IContent
    {
        public string Organization { get; set; }
        public string Author { get; set; }

        public string Title { get; set; }
        public string Summary { get; set; }
        public PieceType Type { get; set; }
        public string Source { get; set; }
        public string Url { get; set; }
        public string[] References { get; set; }
        public DateTime? DatePublished { get; set; }
    }

    public class MyArgument : IContent
    {
        public string Title { get; set; }
        public string Summary { get; set; }
        public PieceType Type { get; set; }
        public string Source { get; set; }
        public string Url { get; set; }
        public string[] References { get; set; }
        public DateTime? DatePublished { get; set; }
    }

    public class BookExcerpt : IContent
    {
        public string Isbn{ get; set; }

        public string Author { get; set; }
        public int FromPage { get; set; }
        public int ToPage { get; set; }

        public string Title { get; set; }
        public string Summary { get; set; }
        public PieceType Type { get; set; }
        public string Source { get; set; }
        public string Url { get; set; }
        public string[] References { get; set; }
        public DateTime? DatePublished { get; set; }

    }
    public class Interview : IContent
    {
        public DateTime DateTaken { get; set; }
        public int FromPage { get; set; }
        public int ToPage { get; set; }
        public Segment[] Segments { get; set; }

        public string Title { get; set; }
        public string Summary { get; set; }
        public PieceType Type { get; set; }
        public string Source { get; set; }
        public string Url { get; set; }
        public string[] References { get; set; }
        public DateTime? DatePublished { get; set; }
    }

    public class Podcast : IContent
    {
        public Segment[] Segments { get; set; }

        public string Title { get; set; }
        public string Summary { get; set; }
        public PieceType Type { get; set; }
        public string Source { get; set; }
        public string Url { get; set; }
        public string[] References { get; set; }
        public DateTime? DatePublished { get; set; }
    }

    public class Study : IContent
    {
        public string Author { get; set; }

        public string Title { get; set; }
        public string Summary { get; set; }
        public PieceType Type { get; set; }
        public string Source { get; set; }
        public string Url { get; set; }
        public string[] References { get; set; }
        public DateTime? DatePublished { get; set; }
    }

    public class Statistic : IContent
    {

        public string Title { get; set; }
        public string Summary { get; set; }
        public PieceType Type { get; set; }
        public string Source { get; set; }
        public string Url { get; set; }
        public string[] References { get; set; }
        public DateTime? DatePublished { get; set; }

    }

    public class Definition : IContent
    {
        public string Title { get; set; }
        public string Summary { get; set; }
        public PieceType Type { get; set; }
        public string Source { get; set; }
        public string Url { get; set; }
        public string[] References { get; set; }
        public DateTime? DatePublished { get; set; }
    }

    public class WebVideo : IContent
    {
        public string Uploader { get; set; }
        public string Channel { get; set; }
        public long Views { get; set; }
        public long Likes{ get; set; }
        public long Dislikes { get; set; }
        public long Comments { get; set; }
        public string Platform { get; set; }
        public Segment[] Segments { get; set; }

        public string Title { get; set; }
        public string Summary { get; set; }
        public PieceType Type { get; set; }
        public string Source { get; set; }
        public string Url { get; set; }
        public string[] References { get; set; }
        public DateTime? DatePublished { get; set; }
    }

    public class Documentary : IContent
    {
        public int Year { get; set; }
        public string Producer { get; set; }
        public Segment[] Segments { get; set; }

        public string Title { get; set; }
        public string Summary { get; set; }
        public PieceType Type { get; set; }
        public string Source { get; set; }
        public string Url { get; set; }
        public string[] References { get; set; }
        public DateTime? DatePublished { get; set; }
    }

    public class WebSearch : IContent
    {
        public string SearchEngine{ get; set; }
        public string Term{ get; set; }
        public string[] Results{ get; set; }

        public string Title { get; set; }
        public string Summary { get; set; }
        public PieceType Type { get; set; }
        public string Source { get; set; }
        public string Url { get; set; }
        public string[] References { get; set; }
        public DateTime? DatePublished { get; set; }
    }

    public class NewsPaperArticle : IContent
    {
        public int IssueNumber { get; set; }
        public int Page { get; set; }

        public string Title { get; set; }
        public string Summary { get; set; }
        public PieceType Type { get; set; }
        public string Source { get; set; }
        public string Url { get; set; }
        public string[] References { get; set; }
        public DateTime? DatePublished { get; set; }
    }
}
