using VideoFromArticle.Models;

namespace VideoFromArticle.Admin.Windows
{
    public class ArticleListBoxItem
    {
        public SlideshowArticle Article { get; }

        public ArticleListBoxItem(SlideshowArticle article)
        {
            Article = article;
        }

        public override string ToString()
        {
            return $"{Article} {Article.Diagnostics()}";
        }
    }
}
