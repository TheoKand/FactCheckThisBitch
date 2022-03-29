using VideoFromArticle.Models;

namespace VideoFromArticle.Admin.Windows
{
    public class ArticleListBoxItem
    {
        public Article Article { get; }

        public ArticleListBoxItem(Article article)
        {
            Article = article;
        }

        public override string ToString()
        {
            return $"{Article}\t{Article.Diagnostics()}";
        }
    }
}
