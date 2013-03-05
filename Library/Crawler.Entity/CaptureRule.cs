using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Husb.Common;

namespace Crawler.Entity
{
    public partial class CaptureRule : IEntity
    {
        public CaptureRule()
        {
            this.RowStatus = 0;
            this.CreatedDate = DateTime.Now;
            //this.XPath = "//div[@id='toolbar']";

            this.ContentType = 1;
            this.ProcessMethod = 1;
            this.MatchMethod = 1;
        }
        public System.Guid ID { get; set; }
        public string Name { get; set; }
        public string StartString { get; set; }
        public string EndString { get; set; }
        public string XPath { get; set; }
        public int ContentType { get; set; }
        public int Category { get; set; }
        public int MatchMethod { get; set; }
        public string MatchRegex { get; set; }
        public int ProcessMethod { get; set; }
        public string Prefix { get; set; }
        public string Suffix { get; set; }
        public string ReplaceRegex { get; set; }
        public int SortOrder { get; set; }
        public string NextPageText { get; set; }
        public string NextPageUrlXPath { get; set; }
        public int MaxPageCount { get; set; }
        public int RowStatus { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string Remark { get; set; }
        //public System.Guid TaskItemId { get; set; }

        public virtual ICollection<TaskItem> TaskItems { get; set; }
        
    }
}
