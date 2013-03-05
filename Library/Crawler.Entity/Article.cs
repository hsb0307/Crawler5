using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Husb.Common;

namespace Crawler.Entity
{
    public partial class Article : IEntity
    {
        public Article()
        {
            this.RowStatus = 0;
            this.CreatedDate = DateTime.Now;
        }
        public System.Guid ID { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string ContentText { get; set; }
        public byte[] ContentHTML { get; set; }
        public string Tags { get; set; }
        public int RowStatus { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string Remark { get; set; }
        public System.Guid TaskItemId { get; set; }
        public int CategoryId { get; set; }

        public virtual TaskItem TaskItem { get; set; }
        public virtual ArticleCategory Category { get; set; }


    }
}
