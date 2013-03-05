using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using Crawler.Entity;
using Husb.Data;

namespace Crawler.Service
{
    public interface IArticleCategoryService : IRepository<ArticleCategory>
    {

    }

    public class ArticleCategoryService : ServiceBase<ArticleCategory>, IArticleCategoryService
    {
        private readonly IRepository<ArticleCategory> articleCategoryRepository;

        public ArticleCategoryService(IRepository<ArticleCategory> articleCategoryRepository)
            : base(articleCategoryRepository)
        {
            this.articleCategoryRepository = articleCategoryRepository;

        }

    }
}
