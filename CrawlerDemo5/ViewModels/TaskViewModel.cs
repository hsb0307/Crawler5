using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CrawlerDemo5.ViewModels
{
    public class TaskViewModel
    {
        public TaskViewModel()
        {
            this.RowStatus = 0;
            this.CreatedDate = DateTime.Now;
            
        }

        public System.Guid ID { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public int TaskType { get; set; }
        public string Encoding { get; set; }
        public string Cookie { get; set; }
        public string LoginUrl { get; set; }
        public bool Executed { get; set; }
        public int RowStatus { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string Remark { get; set; }
    }
}