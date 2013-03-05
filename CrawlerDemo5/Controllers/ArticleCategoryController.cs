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
    public class ArticleCategoryController : Controller
    {
        private readonly IArticleCategoryService categoryService;
        
        public ArticleCategoryController(IArticleCategoryService categoryService)
        {
            this.categoryService = categoryService;
            
        }

        [HttpPost]
        public JsonResult GetPaged(FormCollection forms)
        {
            

            IEnumerable<Crawler.Entity.ArticleCategory> tasks = categoryService.GetAll();//.Table.ToList();
            EasyUIDataGridModel<CategoryViewModel> tms = new EasyUIDataGridModel<CategoryViewModel>();
            tms.rows = tasks.Select(t => new CategoryViewModel { ID = t.ID, Name = t.Name, ParentID = t.ParentID, Path = t.Path, CreatedDate = t.CreatedDate, IsLeaf = t.IsLeaf, QueryCode = t.QueryCode, RowStatus = t.RowStatus });
            // TaskViewModel
            tms.total = 100;

            JsonResult json = Json(tms, JsonRequestBehavior.AllowGet);
            return json;
        }

        //
        // GET: /ArticleCategory/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /ArticleCategory/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /ArticleCategory/Create

        public ActionResult Create()
        {
            ViewBag.ParentID = new SelectList(categoryService.GetAll(), "ID", "Name", 1);
            return View();
        }

        //
        // POST: /ArticleCategory/Create

        [HttpPost]
        public ActionResult Create(ArticleCategory category, string parentID, FormCollection collection)
        {
            if (String.IsNullOrEmpty(parentID))
            {
                category.ParentID = -1;
                category.Path = "/-1/";
                ModelState.Remove("ParentID");
            }
            else
            {
                var parent = categoryService.GetById(category.ParentID);
                category.Path = parent.Path + category.ParentID.ToString() + "/";
            }
            

            if (ModelState.IsValid)
            {
                //category.ID = Guid.NewGuid();
                //db.Tasks.Add(task);
                //db.SaveChanges();
                categoryService.Create(category);

                return RedirectToAction("Index");
            }

            ViewBag.ParentID = new SelectList(categoryService.GetAll(), "ID", "Name", category.ParentID);
            return View(category);

            //try
            //{
            //    // TODO: Add insert logic here

            //    return RedirectToAction("Index");
            //}
            //catch
            //{
            //    return View();
            //}
        }

        //
        // GET: /ArticleCategory/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /ArticleCategory/Edit/5

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
        // GET: /ArticleCategory/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /ArticleCategory/Delete/5

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
