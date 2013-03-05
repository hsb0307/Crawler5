using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CrawlerDemo5.ViewModels
{
    public class TaskItemViewModel
    {
        public TaskItemViewModel()
        {
            this.RowStatus = 0;
            this.CreatedDate = DateTime.Now;
        }

        public System.Guid ID { get; set; }
        public string Url { get; set; }
        public Nullable<bool> IsNavigation { get; set; }
        public Nullable<bool> AutoPaging { get; set; }
        public Nullable<int> PageCategory { get; set; }
        public string NextPage { get; set; }
        public int MaxPageCount { get; set; }
        public string NextPageUrl { get; set; }
        public int RowStatus { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string Remark { get; set; }
        public System.Guid TaskId { get; set; }
    }
}