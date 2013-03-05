using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Husb.Common
{
    [Serializable]
    public partial class DictionaryItem
    {
        /// <summary>
        /// 项的唯一识别号
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 条目英文名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 项友好名称
        /// </summary>
        public string FriendlyName { get; set; }
        /// <summary>
        /// 缩写
        /// </summary>
        public string Abbreviation { get; set; }
        /// <summary>
        /// 排序号
        /// </summary>
        public int OrderNumber { get; set; }
        /// <summary>
        /// 项类别编号
        /// </summary>
        public int CategoryId { get; set; }
        /// <summary>
        /// 项类别
        /// </summary>
        public string Category { get; set; }
        /// <summary>
        /// 项类别有好名称
        /// </summary>
        public string CategoryName { get; set; }
        /// <summary>
        /// 项数据
        /// </summary>
        public string Data { get; set; }
    }
}
