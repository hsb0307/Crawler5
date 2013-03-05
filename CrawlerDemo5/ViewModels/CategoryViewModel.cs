using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CrawlerDemo5.ViewModels
{
    public class CategoryViewModel
    {
        public CategoryViewModel()
        {
            this.RowStatus = 0;
            this.CreatedDate = DateTime.Now;
            
            this.IsLeaf = true;
            this.ParentID = -1;
            this.Path = "/-1/";
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
    }
}