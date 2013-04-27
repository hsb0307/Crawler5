using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.ServiceModel.Syndication;
using System.Runtime.Caching;
using Crawler.Entity;
using Husb.Data;

namespace Crawler.Service
{
    public interface ITaskItemService : IRepository<TaskItem>
    {
        string GetArticle(TaskItem taskItem);

    }

    public class TaskItemService : ServiceBase<TaskItem>, ITaskItemService
    {
        private readonly IRepository<TaskItem> taskItemRepository;
        //private readonly IRepository<Article> articleRepository; // 
        private readonly IArticleService articleRepository;

        public TaskItemService(IRepository<TaskItem> taskItemRepository,
            IArticleService articleRepository)
            : base(taskItemRepository)
        {
            this.taskItemRepository = taskItemRepository;
            this.articleRepository = articleRepository;
        }


        public string GetArticle(TaskItem taskItem)
        {
            Encoding encoding = null;//
            Crawler.Entity.Task task = taskItem.Task;
            if (task.Encoding != null && task.Encoding !="auto")
            {
                encoding = Encoding.GetEncoding(task.Encoding);
            }
            int pageType = task.TaskType;

            CaptureRule rule1 = taskItem.CaptureRules.FirstOrDefault(c => c.Category == 1);// 导航页提取的规则
            CaptureRule rule2 = taskItem.CaptureRules.FirstOrDefault(c => c.Category == 2);// 内容页提取的规则

            taskItem.RowStatus = 1;
            taskItemRepository.Context.Entry<TaskItem>(taskItem).State = System.Data.EntityState.Modified;
            
            //Tuple<string, string, HashSet<String>> tuple = null;
            HashSet<Item> feeds = null;
            string html = "";
            if (taskItem.IsNavigation)
            {
                SaveArticles(taskItem, encoding, taskItem.Url, rule1, rule2, 0, pageType);
            }
            else
            {
                Article a = new Article();
                a.ID = Guid.NewGuid();
                a.Url = taskItem.Url;
                a.TaskItemId = taskItem.ID;
                a.TaskItem = taskItem;
                a.CategoryId = 1;

                if (taskItem.AutoPaging)
                {
                    string title = "";
                    string content = "";
                    GetConsequentArticle(title, content, encoding, taskItem.Url, rule2, taskItem.NextPageUrlXPath, taskItem.NextPageText);
                    a.Name = title;
                    a.ContentText = content;
                }
                else
                {
                    Tuple<string, string> t = GetHtml(encoding, taskItem.Url, rule2);
                    a.Name = t.Item1;
                    a.ContentText = t.Item2;
                }

                articleRepository.Create(a);

                feeds.Add(new Item { Title = a.Name, Url = a.Url });
            }
            feeds = GetFeeds(null);
            foreach (var feed in feeds)
            {
                html += "{\"title\":\"" + feed.Title + "\", \"url\":\"" +  feed.Url + "\"},";
            }
            if (html.Length > 1)
            {
                html = html.Remove(html.Length - 1);
            }
            return html;
        }

        private void SaveArticles(TaskItem taskItem, Encoding encoding, string url, CaptureRule rule1, CaptureRule rule2, int count, int pageType = 1)
        {
            Tuple<HashSet<Item>, string> t = GetUrls(encoding, url, rule1, taskItem.NextPageUrlXPath, taskItem.NextPageText, pageType);

            GetFeeds(t.Item1);

            Tuple<string, string> tuple = null;
            //var content = "";
            foreach (var item in t.Item1)
            {
                if (articleRepository.GetArticleByURL(item.Url) != null) continue; 

                tuple = GetHtml(encoding, item.Url, rule2);
                Article article = new Article();
                article.ID = Guid.NewGuid();
                article.Url = item.Url;
                article.Name = tuple.Item1;
                article.ContentText = ""; //tuple.Item2;
                //article.ContentText = tuple.Item2;
                article.TaskItemId = taskItem.ID;
                article.TaskItem = taskItem;
                article.CategoryId = 1;

                articleRepository.Create(article);

                //WriteLog("<br />" + tuple.Item1 + "<br />" + tuple.Item2, ".htm");
                count++;
            }
            if (count < taskItem.MaxPageCount && !string.IsNullOrEmpty(t.Item2))
            {
                SaveArticles(taskItem, encoding, t.Item2, rule1, rule2,  count, pageType);
            }
        }

        /*
        private void SaveArticles(TaskItem taskItem, Encoding encoding, string url, CaptureRule rule1,  CaptureRule rule2, string nextPageUrlPattern,  int count , int maxCount)
        {
            Tuple<HashSet<String>, string> t = GetUrls(encoding, url, rule1, nextPageUrlPattern, taskItem.NextPageText);
            Tuple<string, string> tuple = null;
            //var content = "";
            foreach (var u in t.Item1)
            {
                tuple = GetHtml(encoding, u, rule2);
                Article article = new Article();
                article.ID = Guid.NewGuid();
                article.Url = u;
                article.Name = tuple.Item1;
                article.ContentText = tuple.Item2;
                article.TaskItemId = taskItem.ID;
                article.TaskItem = taskItem;
                article.CategoryId = 1;

                //articleRepository.Create(article);

                WriteLog("<br />" + tuple.Item1 + "<br />" + tuple.Item2, ".htm");
                count++;
            }
            if (count < maxCount)
            {
                SaveArticles(taskItem, encoding, t.Item2, rule1, rule2, nextPageUrlPattern, count, maxCount);
            }
        }
        */

        private void GetConsequentArticle(string title,  string content,  Encoding encoding, string url, CaptureRule rule, string nextPageUrlPattern, string nextPage)
        {
            Tuple<string, string, string> t = GetHtml(encoding, url, rule, nextPageUrlPattern, nextPage);
            if (t == null)
                return;
            if (title == "")
            {
                title = t.Item1;
            }
            content += "<br /><h2>" + t.Item1 + "</h2>";
            //content += "<br />" + t.Item2 + "<br />";
            WriteLog(content + "<br />" + t.Item2 + "<br />", ".htm");
            content = "";
            if (t.Item3 != null)
            {
                WriteLog(t.Item3 + "\r\n", ".log");
                GetConsequentArticle(title, content, encoding, t.Item3, rule, nextPageUrlPattern, nextPage);
            }

   
        }

        /*
        private static Tuple<HashSet<String>, string> GetUrls(Encoding encoding, string url, CaptureRule rule, string nextPageUrlPattern, string nextPage)
        {
            HashSet<String> urls = new HashSet<string>();
            string nextPageUrl = null;
            if (!String.IsNullOrEmpty(rule.XPath))
            {
                HtmlAgilityPack.HtmlDocument doc = GetHtmlByXPath(encoding, url);
                HtmlAgilityPack.HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes(rule.XPath);
                foreach (var n in nodes)
                {
                    urls.Add(FixUpUrl(url, n.GetAttributeValue("href", "")) );
                }
                HtmlAgilityPack.HtmlNode nextPageNode = null;
                string[] nextPageUrlPatterns = nextPageUrlPattern.Split('|');
                
                string anchorText = null;
                foreach (string p in nextPageUrlPatterns)
                {
                    nextPageNode = doc.DocumentNode.SelectSingleNode(p);
                    if (nextPageNode == null) continue;
                    anchorText = nextPageNode.InnerText;
                    string[] nextPageText = nextPage.Split('|');
                    foreach(string text in nextPageText)
                    {
                        if (text.ToLower() == anchorText.ToLower() ||
                            (text.Substring(0, 3).ToLower() == anchorText.Substring(0, 3).ToLower() && text.Substring(text.Length - 3, 3).ToLower() == anchorText.Substring(text.Length - 3, 3).ToLower()) ||
                           text == "下一页" ||
                           text == "Next&nbsp;Page" ||
                           text == "Next&nbsp;&nbsp;Page")
                        {
                            nextPageUrl = nextPageNode.GetAttributeValue("href", "");
                            nextPageUrl = FixUpUrl(url, nextPageUrl);
                            break;
                        }
                    }
                }

                if (nextPageUrl == null)
                {
                    HtmlAgilityPack.HtmlNode node = doc.DocumentNode.SelectSingleNode(nextPageUrlPattern);
                    if(node != null)
                    {
                        nextPageUrl = node.GetAttributeValue("href", "");
                        nextPageUrl = FixUpUrl(url, nextPageUrl);
                    } 
                }
            }
            else
            {  
                string html = GetHtmlByRegEx(encoding, url);
                html = GetHtml(html, rule.StartString, rule.EndString);
                //string urlPattern = @"(?<=" + rule.StartString + @")[\s\S]*?(?=" + rule.EndString + ")"; // <div class=""post_item_body"">    </h3>
                string achorPattern = @"(?<=<a [^>]+?>)[\s\S]*?(?=</a>)";
                string achorHref = @"(?<=href=[""']?)[\s\S]*?(?=[""']?[ |^>])";

                MatchCollection urlMatches = GetMatches(achorPattern, html);
                foreach (Match match in urlMatches)
                {
                    string subTitle = GetFirstMatch(achorPattern, match.Value.Trim());
                    if (subTitle == null)
                        continue;
                    string href = GetFirstMatch(achorHref, match.Value.Trim());
                    if (href == null)
                        continue;
                    href = href.Trim('"', '/', '\'');
                    urls.Add(href);
                }
                nextPageUrl = GetNextPageUrl(html, url, nextPageUrlPattern, nextPage);
                nextPageUrl = FixUpUrl(url, nextPageUrl);
            }
            Tuple<HashSet<String>, string> tuple = Tuple.Create(urls, nextPageUrl);
            return tuple;
        }
        */
        private static Tuple<HashSet<Item>, string> GetUrls(Encoding encoding, string url, CaptureRule rule, string nextPageUrlPattern, string nextPage, int pageType = 1 )
        {
            HashSet<Item> urls = new HashSet<Item>();
            string nextPageUrl = null;
            if (pageType == 1)
            {
                if (!String.IsNullOrEmpty(rule.XPath))
                {
                    HtmlAgilityPack.HtmlDocument doc = GetHtmlByXPath(encoding, url);
                    HtmlAgilityPack.HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes(rule.XPath);
                    foreach (var n in nodes)
                    {
                        urls.Add(new Item { Url = FixUpUrl(url, n.GetAttributeValue("href", "")), Title = n.InnerText });
                    }
                    HtmlAgilityPack.HtmlNode nextPageNode = null;
                    string[] nextPageUrlPatterns = nextPageUrlPattern.Split('|');

                    string anchorText = null;
                    foreach (string p in nextPageUrlPatterns)
                    {
                        nextPageNode = doc.DocumentNode.SelectSingleNode(p);
                        if (nextPageNode == null) continue;
                        anchorText = nextPageNode.InnerText;
                        string[] nextPageText = nextPage.Split('|');
                        foreach (string text in nextPageText)
                        {
                            if (text.ToLower() == anchorText.ToLower() ||
                                (text.Substring(0, 3).ToLower() == anchorText.Substring(0, 3).ToLower() && text.Substring(text.Length - 3, 3).ToLower() == anchorText.Substring(text.Length - 3, 3).ToLower()) ||
                               text == "下一页" ||
                               text == "Next&nbsp;Page" ||
                               text == "Next&nbsp;&nbsp;Page")
                            {
                                nextPageUrl = nextPageNode.GetAttributeValue("href", "");
                                nextPageUrl = FixUpUrl(url, nextPageUrl);
                                break;
                            }
                        }
                    }

                    if (nextPageUrl == null)
                    {
                        HtmlAgilityPack.HtmlNode node = doc.DocumentNode.SelectSingleNode(nextPageUrlPattern);
                        if (node != null)
                        {
                            nextPageUrl = node.GetAttributeValue("href", "");
                            nextPageUrl = FixUpUrl(url, nextPageUrl);
                        }
                    }
                }
                else
                {
                    string html = GetHtmlByRegEx(encoding, url);
                    html = GetHtml(html, rule.StartString, rule.EndString);
                    //string urlPattern = @"(?<=" + rule.StartString + @")[\s\S]*?(?=" + rule.EndString + ")"; // <div class=""post_item_body"">    </h3>
                    string achorPattern = @"(?<=<a [^>]+?>)[\s\S]*?(?=</a>)";
                    string achorHref = @"(?<=href=[""']?)[\s\S]*?(?=[""']?[ |^>])";

                    MatchCollection urlMatches = GetMatches(achorPattern, html);
                    foreach (Match match in urlMatches)
                    {
                        string subTitle = GetFirstMatch(achorPattern, match.Value.Trim());
                        if (subTitle == null)
                            continue;
                        string href = GetFirstMatch(achorHref, match.Value.Trim());
                        if (href == null)
                            continue;
                        href = href.Trim('"', '/', '\'');
                        urls.Add(new Item { Url = FixUpUrl(url, href), Title = "" });
                    }
                    nextPageUrl = GetNextPageUrl(html, url, nextPageUrlPattern, nextPage);
                    nextPageUrl = FixUpUrl(url, nextPageUrl);
                }
            }
            else {
                SyndicationFeed feed = ReadRssFeed(url);
                foreach (var item in feed.Items)
                {
                    urls.Add(new Item
                    {
                        Url = item.Links.FirstOrDefault().Uri.AbsoluteUri,
                        Title = item.Title.Text,
                        Author = item.Authors.FirstOrDefault().Name,
                        LastUpdatedTime = item.LastUpdatedTime.ToLocalTime()
                    });
                }
            }
            Tuple<HashSet<Item>, string> tuple = Tuple.Create(urls, nextPageUrl);
            return tuple;
        }

        private static SyndicationFeed ReadRssFeed(string url)
        {
            System.Xml.XmlReader reader = System.Xml.XmlReader.Create(url);//http://feeds.feedburner.com/AaronHoffman
            SyndicationFeed feed = SyndicationFeed.Load(reader);
            return feed;
        }

        private static Tuple<string, string> GetHtml(Encoding encoding, string url, CaptureRule rule)
        {
            string html = null;
            string title = null;
            Tuple<string, string> tuple = null;
            if (!String.IsNullOrEmpty(rule.XPath))
            {
                HtmlAgilityPack.HtmlWeb htmlWeb = new HtmlAgilityPack.HtmlWeb();
                HtmlAgilityPack.HtmlDocument doc = null;
                if (encoding != null)
                {
                    htmlWeb.AutoDetectEncoding = false;
                    htmlWeb.OverrideEncoding = encoding;
                }
                try
                {
                    doc = htmlWeb.Load(url);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {

                }
                string[] xpaths = rule.XPath.Split('|');

                foreach (string path in xpaths)
                {
                    if(String.IsNullOrEmpty( path)) continue;
                    HtmlAgilityPack.HtmlNode node = doc.DocumentNode.SelectSingleNode(rule.XPath);
                    tuple = new Tuple<string, string>(
                        doc.DocumentNode.SelectSingleNode("//title").InnerText,
                        node.InnerHtml);
                }
            }
            else
            {
                Func<object, string> t4 = uri =>
                {
                    using (var wc = new System.Net.WebClient())
                    {
                        wc.Encoding = Encoding.UTF8;
                        return wc.DownloadString((String)uri);
                    }
                };

                System.Threading.Tasks.Task<string> task = System.Threading.Tasks.Task.Factory.StartNew<string>(() => t4(url));
                html = task.Result;

                Regex tmpRegex = new Regex(@"(?<=<title>)[\s\S]*?(?=</title>)", RegexOptions.IgnoreCase);
                MatchCollection matches = tmpRegex.Matches(html);
                if (matches.Count > 0)
                {
                    title = matches[0].Value;
                }
                html = GetHtml(html, rule.StartString, rule.EndString);
                tuple = new Tuple<string, string>(html,title);
            }
            return tuple;
        }
       
        private static string GetHtml(string html, string startString, string endString)
        {
            string pattern = @"(?<=" + startString + @")[\s\S]*?(?=" + endString + ")";
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
            MatchCollection mches = regex.Matches(html);
            if (mches.Count > 0)
            {
                html = mches[0].Value;
            }
            else
            {
                int start = html.IndexOf(startString);
                int end = html.LastIndexOf(endString);
                if (start >= 0 && end > 0 && end <= html.Length)
                {
                    html = html.Substring(start + startString.Length, end - start - endString.Length);
                }
            }
            return html;
        }

        private static HtmlAgilityPack.HtmlDocument GetHtmlByXPath(Encoding encoding, string url)
        {
            HtmlAgilityPack.HtmlWeb htmlWeb = new HtmlAgilityPack.HtmlWeb();
            HtmlAgilityPack.HtmlDocument doc = null;
            if (encoding != null)
            {
                htmlWeb.AutoDetectEncoding = false;
                htmlWeb.OverrideEncoding = encoding;
            }
            try
            {
                doc = htmlWeb.Load(url);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
            return doc;
        }

        private static string GetHtmlByRegEx(Encoding encoding, string url)
        {
            Func<object, string> t4 = uri =>
            {
                using (var wc = new System.Net.WebClient())
                {
                    wc.Encoding = encoding; //Encoding.UTF8;
                    return wc.DownloadString((String)uri);
                }
            };

            System.Threading.Tasks.Task<string> task = System.Threading.Tasks.Task.Factory.StartNew<string>(() => t4(url));
            return task.Result;
            //return null;
        }

        private static Tuple<string, string, string> GetHtml(Encoding encoding, string url, CaptureRule rule, string nextPageUrlPattern, string nextPage)
        {
            string html = null;
            string title = null;
            Tuple<string, string, string> tuple = null;
            if (!String.IsNullOrEmpty(rule.XPath))
            {

                HtmlAgilityPack.HtmlDocument doc = GetHtmlByXPath(encoding, url);
                HtmlAgilityPack.HtmlNode node = doc.DocumentNode.SelectSingleNode(rule.XPath);
                if (node == null)
                    return null;

                tuple = new Tuple<string, string, string>(
                    doc.DocumentNode.SelectSingleNode("//title").InnerText,
                    node.InnerHtml,
                    GetNextPageUrl(doc,url, nextPageUrlPattern, nextPage) );
            }
            else
            {

                html = GetHtmlByRegEx(encoding, url);
                Regex tmpRegex = new Regex(@"(?<=<title>)[\s\S]*?(?=</title>)", RegexOptions.IgnoreCase);
                MatchCollection matches = tmpRegex.Matches(html);
                if (matches.Count > 0)
                {
                    title = matches[0].Value;
                }
                html = GetHtml(html, rule.StartString, rule.EndString);
                tuple = new Tuple<string, string, string>(html, title, GetNextPageUrl(html,  url,  nextPageUrlPattern,  nextPage));
            }
            return tuple;
        }

        private static string GetNextPageUrl(HtmlAgilityPack.HtmlDocument doc, string url, string nextPageUrlPattern, string nextPage)
        {
            string nextPageUrl = null;
            HtmlAgilityPack.HtmlNode nextPageNode = null;
            string[] nextPageUrlPatterns = nextPageUrlPattern.Split('|');

            string anchorText = null;
            foreach (string p in nextPageUrlPatterns)
            {
                if(!String.IsNullOrEmpty(nextPageUrl))
                    break;
                nextPageNode = doc.DocumentNode.SelectSingleNode(p);
                if (nextPageNode == null) continue;
                anchorText = nextPageNode.InnerText;
                string[] nextPageText = nextPage.Split('|');
                foreach (string text in nextPageText)
                {
                    if (text.ToLower() == anchorText.ToLower() ||
                        (text.Substring(0, 3).ToLower() == anchorText.Substring(0, 3).ToLower() && text.Substring(text.Length - 3, 3).ToLower() == anchorText.Substring(text.Length - 3, 3).ToLower()) ||
                       text == "下一页" ||
                       text == "Next&nbsp;Page" ||
                       text == "Next&nbsp;&nbsp;Page")
                    {
                        nextPageUrl = nextPageNode.GetAttributeValue("href", "");
                        nextPageUrl = FixUpUrl(url, nextPageUrl);
                        break;
                    }
                }
            }

            if (nextPageUrl == null)
            {
                HtmlAgilityPack.HtmlNode node = doc.DocumentNode.SelectSingleNode(nextPageUrlPattern);
                if (node != null)
                {
                    nextPageUrl = node.GetAttributeValue("href", "");
                    nextPageUrl = FixUpUrl(url, nextPageUrl);
                }
            }
            return nextPageUrl;
        }

        private static string GetNextPageUrl(string html, string url, string nextPageUrlPattern, string nextPage)
        {
            return null;
        }

        private static string GetFirstMatch(string pattern, string originalText)
        {
            MatchCollection matches = GetMatches(pattern, originalText);
            if (matches.Count > 0)
            {
                return matches[0].Value;
            }
            else
            {
                return null;
            }
        }

        private static MatchCollection GetMatches(string pattern, string originalText)
        {
            Regex tmpRegex = new Regex(pattern, RegexOptions.IgnoreCase);
            MatchCollection matches = tmpRegex.Matches(originalText);

            return matches;
        }

        public static string FixUpUrl(string baseUrl, string originalUrl)
        {

            if (originalUrl == null || originalUrl.Length == 0)
                return originalUrl;
            if (originalUrl.StartsWith("http://", StringComparison.OrdinalIgnoreCase))
                return originalUrl;
            if (baseUrl == null || baseUrl.Length == 0)
                return originalUrl;

            string tmp = baseUrl.Substring(0, baseUrl.LastIndexOf('/'));

            if (originalUrl[0] == '/')
            {
                if (baseUrl.StartsWith("http://", StringComparison.OrdinalIgnoreCase))
                {
                    return baseUrl.Substring(0, baseUrl.IndexOf('/', 7)) + originalUrl;
                }
                else
                {
                    return "http://" + baseUrl.Substring(0, baseUrl.IndexOf('/')) + originalUrl;
                }
            }

            if (originalUrl[0] == '.')
            {
                if (originalUrl[1] == '.')
                {
                    while (originalUrl.StartsWith("../"))
                    {
                        originalUrl = originalUrl.Substring(3);
                        tmp = tmp.Substring(0, tmp.LastIndexOf('/'));
                    }
                    return tmp + "/" + originalUrl;
                }
                else
                {
                    return tmp + originalUrl.Substring(1);
                }
            }


            return tmp + "/" + originalUrl;
        }

        private static void WriteLog(string message, string ext)
        {

            #region Log
            string path = "";
            if (System.Environment.CurrentDirectory == AppDomain.CurrentDomain.BaseDirectory)
            {
                path = AppDomain.CurrentDomain.BaseDirectory;
            }
            else
            {
                path = AppDomain.CurrentDomain.BaseDirectory + "App_Data\\";
            }

            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }
            string logFile = path + DateTime.Now.ToString("yyyyMMddHH") + ext;// yyyyMMddHHmmss
            if (!System.IO.File.Exists(logFile))
            {
                System.IO.FileStream log = System.IO.File.Create(logFile);
                log.Close();
            }
            #endregion

            using (System.IO.StreamWriter w = System.IO.File.AppendText(logFile))
            {
                w.WriteLine(message);
            }
        }

        private static HashSet<Item> GetFeeds(HashSet<Item> feeds)
        {
            ObjectCache cache = MemoryCache.Default;
            HashSet<Item> fullFeeds = cache["DictionaryData"] as HashSet<Item>;

            if (fullFeeds == null)
            {
                CacheItemPolicy policy = new CacheItemPolicy();
                policy.AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(5.0);
                if (feeds != null)
                {
                    cache.Set("DictionaryData", feeds, policy);
                }
            }
            else
            {
                if (feeds != null)
                {
                    fullFeeds.UnionWith(feeds);
                }
            }
            return fullFeeds;
        }

        private class Item
        {
            public string Url { get; set; }
            public string Title { get; set; }
            public string Author { get; set; }
            public DateTimeOffset LastUpdatedTime { get; set; }
        }

    }
}
