using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CrawlerDemo5.ViewModels
{
    public class CaptureRuleViewModel
    {
        public CaptureRuleViewModel()
        {
            this.RowStatus = 0;
            this.CreatedDate = DateTime.Now;
        }
        public System.Guid ID { get; set; }
        public string Name { get; set; }
        public string StartString { get; set; }
        public string EndString { get; set; }
        public string XPath { get; set; }
        public Nullable<int> ContentType { get; set; }
        public int Category { get; set; }
        public Nullable<int> MatchMethod { get; set; }
        public string MatchRegex { get; set; }
        public Nullable<int> ProcessMethod { get; set; }
        public string Prefix { get; set; }
        public string Suffix { get; set; }
        public string ReplaceRegex { get; set; }
        public int SortOrder { get; set; }
        public string NextPage { get; set; }
        public string NextPageUrl { get; set; }
        public int MaxPageCount { get; set; }
        public int RowStatus { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string Remark { get; set; }
    }
}