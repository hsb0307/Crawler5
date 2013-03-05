using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CrawlerDemo5.ViewModels
{
    public class EasyUIDataGridModel<T>
    {
        private int _total;
        private IEnumerable<T> _rows;
        //private List<T> _footers;

        public int total
        {
            get { return _total; }
            set { _total = value; }
        }

        public IEnumerable<T> rows
        {
            get { return _rows; }
            set { _rows = value; }
        }
    }
}