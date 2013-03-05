using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Husb.Common;

namespace Crawler.Entity
{
    public partial class Task : IEntity
    {
        public Task()
        {
            this.RowStatus = 0;
            this.CreatedDate = DateTime.Now;
            this.TaskItems = new HashSet<TaskItem>();

            this.CategoryId = 1;
            this.TaskType = 1;
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

        public virtual TaskCategory TaskCategory { get; set; }
        public virtual ICollection<TaskItem> TaskItems { get; set; }
    }
}
