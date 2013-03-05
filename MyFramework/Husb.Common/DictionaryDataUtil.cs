using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Runtime.Caching;

namespace Husb.Common
{
    public class DictionaryDataUtil
    {

        public static List<DictionaryItem> GetData(params string[] keys)
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            //if (System.Environment.CurrentDirectory == AppDomain.CurrentDomain.BaseDirectory)
            //{
            //    basePath = AppDomain.CurrentDomain.BaseDirectory;
            //}
            //else
            //{
            //    basePath = AppDomain.CurrentDomain.BaseDirectory;// +"Bin\\";
            //}
            var dictionaryDataFilename = basePath + @"App_Data\EnumerableData.xml";

            ObjectCache cache = MemoryCache.Default;
            List<DictionaryItem> dictionaryData = cache["DictionaryData"] as List<DictionaryItem>;

            if (dictionaryData == null)
            {
                CacheItemPolicy policy = new CacheItemPolicy();
                //policy.AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(10.0);
                policy.AbsoluteExpiration = DateTimeOffset.Now.AddHours(24.0);

                List<string> filePaths = new List<string>();
                filePaths.Add(dictionaryDataFilename);

                policy.ChangeMonitors.Add(new HostFileChangeMonitor(filePaths));

                // Fetch the file contents.
                dictionaryData = GetDictionaryData(dictionaryDataFilename);

                cache.Set("DictionaryData", dictionaryData, policy);
            }
            if (keys == null)
            {
                return dictionaryData;
            }
            else
            {
                List<DictionaryItem> data = new List<DictionaryItem>();
                foreach(string key in keys)
                {
                    data.AddRange(dictionaryData.Where(d => d.Category == key));
                }

                return data;
            }
        }

        public static DictionaryItem GetDictionaryItem(string categoryKey, int id)
        {
            List<DictionaryItem> categories = GetData(categoryKey);
            return categories.FirstOrDefault(d => d.ID == id);
        }

        public static List<DictionaryItem> GetDictionaryData(string dictionaryDataFilename)
        {
            //try
            //{

            //    XDocument doc = XDocument.Load(dictionaryDataFilename);

            //    var query = from root in doc.Elements()
            //                from category in root.Elements()
            //                from item in category.Elements()
            //                select new DictionaryItem
            //                {
            //                    ID = Int32.Parse(item.Attribute("id").Value),
            //                    Name = item.Attribute("name").Value,
            //                    FriendlyName = item.Attribute("friendlyName") == null ? null : item.Attribute("friendlyName").Value,
            //                    OrderNumber = item.Attribute("orderNumber") == null ? 0 : Int32.Parse(item.Attribute("orderNumber").Value),
            //                    Abbreviation = item.Attribute("abbreviation") == null ? null : item.Attribute("abbreviation").Value,
            //                    Data = item.Attribute("data") == null ? null : item.Attribute("data").Value,
            //                    CategoryId = Int32.Parse(category.Attribute("id").Value),
            //                    CategoryName = category.Attribute("friendlyName").Value,
            //                    Category = category.Name.LocalName //Attribute("friendlyName").Value
            //                };
            //    return query.ToList();
            //}
            //catch
            //{
            //    return new List<DictionaryItem>() { new DictionaryItem { ID = -1, Name = "字典文件不存在或者格式不正确！" } };
            //}
            XDocument doc = XDocument.Load(dictionaryDataFilename);

            var query = from root in doc.Elements()
                        from category in root.Elements()
                        from item in category.Elements()
                        select new DictionaryItem
                        {
                            ID = Int32.Parse(item.Attribute("id").Value),
                            Name = item.Attribute("name").Value,
                            FriendlyName = item.Attribute("friendlyName") == null ? null : item.Attribute("friendlyName").Value,
                            OrderNumber = item.Attribute("orderNumber") == null ? 0 : Int32.Parse(item.Attribute("orderNumber").Value),
                            Abbreviation = item.Attribute("abbreviation") == null ? null : item.Attribute("abbreviation").Value,
                            Data = item.Attribute("data") == null ? null : item.Attribute("data").Value,
                            CategoryId = Int32.Parse(category.Attribute("id").Value),
                            CategoryName = category.Attribute("friendlyName").Value,
                            Category = category.Name.LocalName //Attribute("friendlyName").Value
                        };
            return query.ToList();
        }

        
    }
}
