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
    public class TaskController : Controller
    {
        private readonly ITaskService taskService;
        private readonly ITaskCategoryService categoryService;
        
        public TaskController(ITaskService taskService,
            ITaskCategoryService categoryService)
        {
            this.taskService = taskService;
            this.categoryService = categoryService;
            
        }

        [HttpPost]
        public JsonResult GetPaged(int pageIndex, FormCollection forms)
        {
            IEnumerable<Crawler.Entity.Task> tasks = taskService.Table.ToList();
            EasyUIDataGridModel<TaskViewModel> tms = new EasyUIDataGridModel<TaskViewModel>();
            tms.rows = tasks.Select(t => new TaskViewModel { ID = t.ID, Name = t.Name, CategoryId = t.CategoryId, Cookie = t.Cookie, CreatedDate = t.CreatedDate, Encoding = t.Encoding, LoginUrl = t.LoginUrl, TaskType = t.TaskType });
            // TaskViewModel
            tms.total = 100;

            JsonResult json = Json(tms, JsonRequestBehavior.AllowGet);
            return json;
        }
        //
        // GET: /Task/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Task/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Task/Create

        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(categoryService.Table.ToList(), "ID", "Name", 1);
            return View();
        }

        //
        // POST: /Task/Create

        [HttpPost]
        public ActionResult Create(Crawler.Entity.Task task)
        {
            if (ModelState.IsValid)
            {
                task.ID = Guid.NewGuid();
                //db.Tasks.Add(task);
                //db.SaveChanges();
                taskService.Create(task);
                
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(categoryService.Table.ToList(), "ID", "Name", task.CategoryId);//db.TaskCategories
            return View(task);

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
        // GET: /Task/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Task/Edit/5

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
        // GET: /Task/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Task/Delete/5

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
