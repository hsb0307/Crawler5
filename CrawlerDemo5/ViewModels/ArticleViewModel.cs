using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CrawlerDemo5.ViewModels
{
    public class ArticleViewModel
    {
        public Guid ID { get; set; }
        public String Name { get; set; }
        public String Url { get; set; }
        public String Tags { get; set; }
        public Int32 RowStatus { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid TaskItemId { get; set; }
    }
}