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

            SyndicationFeed feed = ReadRssFeed("http://feed.cnblogs.com/blog/sitehome/rss");
            //SyndicationFeed feed = ReadRssFeed("http://feeds.feedburner.com");//
            return View(feed);
        }

        public ActionResult Open(string url)
        {
            ViewBag.Html = GetHtml(System.Text.UTF8Encoding.UTF8, url);
            return View();
        }

        // GetItems
        public ActionResult GetItems(string txtUrl)
        {
            SyndicationFeed feed = ReadRssFeed(txtUrl);
            return View(feed);
        }

        public ActionResult GetHtml(string txtHttp)
        {
            ViewBag.Html = GetHtml(System.Text.UTF8Encoding.UTF8, txtHttp);
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
    }
}
