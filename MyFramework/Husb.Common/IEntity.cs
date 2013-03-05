using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Husb.Common
{
    //public class IEntity<V>
    //{
    //    V ID { get; set; }
    //    int RowStatus { get; set; }
    //    DateTime CreatedDate { get; set; }
    //    string Remark { get; set; }
    //}
    public interface IEntity
    {
        //object ID { get; set; }
        int RowStatus { get; set; }
        DateTime CreatedDate { get; set; }
        string Remark { get; set; }
    }

    //public interface IEntity
    //{
    //    public object ID { get; set; }
    //    public int RowStatus { get; set; }
    //    public DateTime CreatedDate { get; set; }
    //    public string Remark { get; set; }
    //}
}
