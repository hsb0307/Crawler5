using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Husb.Common;

namespace Crawler.Entity
{
    public partial class TaskItem : IEntity
    {
        public TaskItem()
        {
            this.IsNavigation = false;
            this.AutoPaging = false;
            this.PageCategory = 1;
            this.RowStatus = 0;
            this.CreatedDate = DateTime.Now;

            this.CaptureRules = new HashSet<CaptureRule>();
        }

        //private Guid id;
        //public Guid ID { get { return id; } set { value = id; } }

        public System.Guid ID { get; set; }
        public string Url { get; set; }
        public bool IsNavigation { get; set; }
        public bool AutoPaging { get; set; }
        public int PageCategory { get; set; }
        public string NextPageText { get; set; }
        public int MaxPageCount { get; set; }
        public string NextPageUrlXPath { get; set; }
        public int RowStatus { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string Remark { get; set; }
        public System.Guid TaskId { get; set; }
        public int CategoryId { get; set; }

        public virtual ICollection<CaptureRule> CaptureRules { get; set; }
        public virtual Task Task { get; set; }
        public virtual Article Article { get; set; }
        //public virtual ArticleCategory Category { get; set; }
    }
}
