using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Husb.Common;

namespace Crawler.Entity
{
    public partial class ArticleCategory : IEntity
    {
        public ArticleCategory()
        {
            this.RowStatus = 0;
            this.CreatedDate = DateTime.Now;
            this.Articles = new HashSet<Article>();
            //this.TaskItems = new HashSet<TaskItem>();
        }

        public int ID { get; set; }
        public string Name { get; set; }
        
        public string QueryCode { get; set; }
        public int ParentID { get; set; }
        public string Path { get; set; }
        public bool IsLeaf { get; set; }
        public int RowStatus { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string Remark { get; set; }

        public virtual ICollection<Article> Articles { get; set; }

        //public virtual ICollection<TaskItem> TaskItems { get; set; }
    }
}
