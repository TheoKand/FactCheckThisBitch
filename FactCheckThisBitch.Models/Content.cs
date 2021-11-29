using System;
using System.Collections.Generic;
using System.Text;

namespace FactCheckThisBitch.Models
{
    public abstract class BaseContent
    {
        public string Title;
        public string Summary;
        public PieceType Type;
        public string Source;
        public string Url;
        public string[] References;
        public DateTime? DatePublished;
    }

    public class Article : BaseContent
    {
        public Organisation Organization;
        public string Author;
    }

    public class MyArgument : BaseContent
    {
    }

    public class BookExcerpt : BaseContent
    {
        public string Isbn;
        public string Author;
        public int FromPage;
        public int ToPage;

    }
    public class Interview : BaseContent
    {
        public DateTime DateTaken;
        public int FromPage;
        public int ToPage;
        public Segment[] Segments;
    }

    public class Podcast : BaseContent
    {
        public Segment[] Segments;
    }

    public class Study : BaseContent
    {
        public string Author;
    }

    public class Statistic : BaseContent
    {

    }

    public class Definition : BaseContent
    {

    }

    public class WebVideo : BaseContent
    {
        public string Uploader;
        public string Channel;
        public long Views;
        public long Likes;
        public long Dislikes;
        public long Comments;
        public Organisation Platform;
        public Segment[] Segments;
    }

    public class Documentary : BaseContent
    {
        public int Year;
        public string Producer;
        public Segment[] Segments;
    }

    public class WebSearch : BaseContent
    {
        public string SearchEngine;
        public string Term;
        public string[] Results;
    }

    public class NewsPaperArticle : BaseContent
    {
        public int IssueNumber;
        public int Page;
    }

    public class Comparisson : BaseContent
    {
        public BaseContent Fact1;
        public BaseContent Face2;
    }


}
