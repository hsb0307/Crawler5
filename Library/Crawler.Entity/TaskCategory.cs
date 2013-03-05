using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Husb.Common;

namespace Crawler.Entity
{

    public partial class TaskCategory : IEntity
    {
        public TaskCategory()
        {
            this.RowStatus = 0;
            this.CreatedDate = DateTime.Now;
            this.IsLeaf = true;
            this.ParentID = -1;
            this.Path = "/-1/";
            this.Tasks = new HashSet<Task>();
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

        public virtual ICollection<Task> Tasks { get; set; }
    }
}
