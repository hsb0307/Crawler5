using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Crawler.Entity;
using Crawler.Service;
using CrawlerDemo5.ViewModels;

namespace CrawlerDemo5.Controllers
{
    public class ArticleController : Controller
    {
        private readonly IArticleService articleService;
        private readonly ITaskItemService taskItemService;

        public ArticleController(IArticleService articleService,
            ITaskItemService taskItemService)
        {
            this.articleService = articleService;
            this.taskItemService = taskItemService;
        }
        //
        // GET: /Article/

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetPaged(FormCollection forms, Guid? taskid)
        {
            //IList<Crawler.Entity.Article> articles = (taskid == null ? articleService.GetAll() : articleService.GetByMasterId(taskid.Value)) as IList<Crawler.Entity.Article>;

            //Expression<Func<Employee, bool>> criteria = GetCriteria(forms);
            int pageIndex = Int32.Parse(forms["page"]) - 1;
            int pageSize = Int32.Parse(forms["rows"]);
            int recordCount = articleService.Query.Count();
            int pageCount = recordCount / pageSize;
            if (recordCount % pageSize != 0) pageCount += 1;

            int maximumRows = pageSize;
            int startRowIndex = pageIndex * pageSize;

            IEnumerable<Crawler.Entity.Article> articles = articleService.Query.OrderByDescending(a => a.CreatedDate).Skip(startRowIndex).Take(maximumRows).ToList();// as IList<Crawler.Entity.Article>;
            IList<ArticleViewModel> models = new List<ArticleViewModel>();
            foreach (var a in articles)
            {
                models.Add(new ArticleViewModel { ID = a.ID, Name = a.Name.Trim(), Url = a.Url, Tags = a.Tags, CreatedDate = a.CreatedDate, RowStatus = a.RowStatus, TaskItemId = a.TaskItemId });
            }
            EasyUIDataGridModel<ArticleViewModel> tms = new EasyUIDataGridModel<ArticleViewModel>();
            tms.rows = models;
            tms.total = recordCount;

            JsonResult json = Json(tms, JsonRequestBehavior.AllowGet);
            return json;
        }

        //
        // GET: /Article/Details/5

        public ActionResult Details(Guid id)
        {
            Crawler.Entity.Article article = articleService.GetById(id);//
            if (article == null)
            {
                return HttpNotFound();
            }
            return View(article);
        }

        //
        // GET: /Article/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Article/Create

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
        // GET: /Article/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Article/Edit/5

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
        // GET: /Article/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Article/Delete/5

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
    }
}
