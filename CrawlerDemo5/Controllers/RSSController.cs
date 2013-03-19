using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.ServiceModel.Syndication;
using System.Xml;

namespace CrawlerDemo5.Controllers
{
    public class RSSController : Controller
    {
        //
        // GET: /RSS/

        public ActionResult Index()
        {
            // http://erikej.blogspot.com/feeds/posts/default
            // http://feed.cnblogs.com/blog/sitehome/rss
            // http://feeds.feedburner.com/AaronHoffman
            // http://www.cnbeta.com/backend.php

            //The underlying connection was closed: An unexpected error occurred on a receive.
            SyndicationFeed feed = null;
            
            //try
            //{
            //    feed = ReadRssFeed("http://feed.cnblogs.com/blog/sitehome/rss");
            //}
            //catch (Exception ex)
            //{
            //    ViewBag.Message = ex.Message;
            //}

            //SyndicationFeed feed = ReadRssFeed("http://feeds.feedburner.com");//
            return View(feed);
        }

        public ActionResult Open(string url)
        {
            url =  System.Text.Encoding.UTF8.GetString( Convert.FromBase64String( url));
            ViewBag.Html = GetHtml(System.Text.UTF8Encoding.UTF8, url);
            return View();
        }

        // GetItems
        public ActionResult GetItems(string txtUrl)
        {
            SyndicationFeed feed = null;
            try
            {
                feed = ReadRssFeed(txtUrl);
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
            }
            return View(feed);
        }

        public ActionResult GetHtml(string txtHttp)
        {
            try
            {
                //ViewBag.Html = GetHtml(System.Text.UTF8Encoding.UTF8, txtHttp);
                ViewBag.Html = GetHtmlByXPath(null, txtHttp);
            }
            catch (Exception ex)
            {
                ViewBag.Html = "<h2> 惨了，打不开" + txtHttp + " 啊!</h2><div>" + ex.Message + "</div>"; 
            }
            return View("Open");
        }

        public ActionResult GetWallOut(string txtWallOut)
        {
            string url = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(txtWallOut));
            try
            {
                ViewBag.Html = GetHtmlByXPath(null, url);
                //ViewBag.Html = GetHtml(System.Text.UTF8Encoding.UTF8, url);
            }
            catch (Exception ex)
            {
                ViewBag.Html = "<h2> 惨了，打不开" + url + " 啊，已经穿墙了啊！</h2><div>" + ex.Message + "</div>";
            }
            return View("Open");
        }

        //
        // GET: /Rss/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Rss/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Rss/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Rss/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Rss/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Rss/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Rss/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        private static string GetHtml(System.Text.Encoding encoding, string url)
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

        private static SyndicationFeed ReadRssFeed(string url)
        {
            XmlReader reader = XmlReader.Create(url);//http://feeds.feedburner.com/AaronHoffman
            SyndicationFeed feed = SyndicationFeed.Load(reader);
            return feed;
        }

        private static string GetHtmlByXPath(System.Text.Encoding encoding, string url)
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
            if (doc == null)
                return "";
            else
                return doc.DocumentNode.InnerHtml;
            //return doc;
        }
    }
}
