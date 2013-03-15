using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crawler.Entity;
using Husb.Data;

namespace Crawler.Service
{
    public interface IArticleService : IRepository<Article>
    {
        Article GetArticleByURL(string url);

    }

    public class ArticleService : ServiceBase<Article>, IArticleService
    {
        private readonly IRepository<Article> articleRepository;

        public ArticleService(IRepository<Article> articleRepository)
            : base(articleRepository)
        {
            this.articleRepository = articleRepository;

        }

        public Article GetArticleByURL(string url)
        {
            return articleRepository.Query.SingleOrDefault(a => a.Url == url);
        }

    }
}
