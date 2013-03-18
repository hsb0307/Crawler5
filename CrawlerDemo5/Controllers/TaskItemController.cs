using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Husb.Common;
using Crawler.Entity;
using Crawler.Service;
using CrawlerDemo5.ViewModels;

namespace CrawlerDemo5.Controllers
{
    public class TaskItemController : Controller
    {
        private readonly ITaskService taskService;
        private readonly ITaskItemService taskItemService;
        private readonly ICaptureRuleService captureRuleService;
        
        public TaskItemController(ITaskService taskService,
            ITaskItemService taskItemService,
            ICaptureRuleService captureRuleService)
        {
            this.taskService = taskService;
            this.taskItemService = taskItemService;
            this.captureRuleService = captureRuleService;
            
        }

        [HttpPost]
        public JsonResult GetPaged(FormCollection forms, string taskid)
        {
            int pageIndex = Int32.Parse(forms["page"]) - 1;
            int pageSize = Int32.Parse(forms["rows"]);
            int recordCount = taskItemService.Query.Count();
            int pageCount = recordCount / pageSize;
            if (recordCount % pageSize != 0) pageCount += 1;

            int maximumRows = pageSize;
            int startRowIndex = pageIndex * pageSize;

            var query = String.IsNullOrEmpty(taskid) ? taskItemService.Query.OrderByDescending(ti => ti.CreatedDate): taskItemService.Query.Where(i => i.TaskId == new Guid(taskid)).OrderByDescending(ti => ti.CreatedDate);
            IEnumerable<Crawler.Entity.TaskItem> tasks = query.Skip(startRowIndex).Take(maximumRows);
            EasyUIDataGridModel<TaskItemViewModel> tms = new EasyUIDataGridModel<TaskItemViewModel>();
            tms.rows = tasks.Select(t => new TaskItemViewModel
            {
                ID = t.ID,
                AutoPaging = t.AutoPaging,
                CreatedDate = t.CreatedDate,
                IsNavigation = t.IsNavigation,
                MaxPageCount = t.MaxPageCount,
                NextPage = t.NextPageText,
                NextPageUrl = t.NextPageUrlXPath,
                PageCategory = t.PageCategory,
                RowStatus = t.RowStatus,
                TaskId = t.TaskId,
                Url = t.Url
            });
            // TaskViewModel
            tms.total = recordCount;

            JsonResult json = Json(tms, JsonRequestBehavior.AllowGet);
            return json;
        }

        [HttpPost]
        public ContentResult Execute(Guid id)
        {
            Crawler.Entity.TaskItem taskItem = taskItemService.GetById(id);

            //try
            //{
            //    string article = taskItemService.GetArticle(taskItem);
            //    return new ContentResult { Content = article };
            //}
            //catch (Exception ex)
            //{
            //    throw new Exception(ex.Message);
            //    //return new ContentResult { Content = ex.Message  };
            //}
            string article = taskItemService.GetArticle(taskItem);
            return new ContentResult { Content = article };
        }

        //
        // GET: /TaskItem/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /TaskItem/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /TaskItem/Create

        public ActionResult Create(string taskid)
        {
            if (String.IsNullOrEmpty(taskid))
            {
                ViewBag.TaskId = new SelectList(taskService.GetAll(), "ID", "Name");
            }
            
            else
            {
                ViewBag.TaskId = new SelectList(taskService.GetAll(), "ID", "Name", taskid);
            }
            IArticleCategoryService articleCategoryService = DependencyResolver.Current.GetService<IArticleCategoryService>();
            ViewBag.CategoryId = new SelectList(articleCategoryService.GetAll(), "ID", "Name");

            ViewBag.CaptureRuleForContent = new SelectList(captureRuleService.GetAll(), "ID", "Name");
            ViewBag.CaptureRuleForNavigation = new SelectList(captureRuleService.GetAll(), "ID", "Name");

            ViewBag.PageCategory = new SelectList(DictionaryDataUtil.GetData("pageType"), "ID", "Name", 1);
            return View();
        }

        //
        // POST: /TaskItem/Create

        [HttpPost]
        public ActionResult Create(Crawler.Entity.TaskItem item, string createAddCaptureRule, string captureRuleForContent, string captureRuleForNavigation)
        {
            if (ModelState.IsValid)
            {
                item.ID = Guid.NewGuid();

                if (!string.IsNullOrEmpty(captureRuleForContent))
                {
                    Crawler.Entity.CaptureRule captureRuleCon = new Crawler.Entity.CaptureRule { ID = new Guid(captureRuleForContent) };
                    captureRuleService.Context.Set<Crawler.Entity.CaptureRule>().Attach(captureRuleCon);
                    item.CaptureRules.Add(captureRuleCon);
                }
                if (!string.IsNullOrEmpty(captureRuleForNavigation))
                {
                    CaptureRule captureRuleNav = new CaptureRule { ID = new Guid(captureRuleForNavigation) };
                    captureRuleService.Context.Set<CaptureRule>().Attach(captureRuleNav);
                    item.CaptureRules.Add(captureRuleNav);
                }

                taskItemService.Create(item);
                if (string.IsNullOrEmpty(createAddCaptureRule))
                {
                    return RedirectToAction("Index", new { taskid = item.TaskId });
                }
                else
                {
                    return RedirectToAction("Create", "CaptureRule", new { taskitemid = item.ID });
                }
                
            }

            ViewBag.TaskId = new SelectList(taskService.GetAll(), "ID", "Name", item.TaskId);
            ViewBag.PageCategory = new SelectList(DictionaryDataUtil.GetData("pageType"), "ID", "Name", 1);
            return View(item);


        }

        //
        // GET: /TaskItem/Edit/5

        public ActionResult Edit(Guid id)
        {
            Crawler.Entity.TaskItem taskitem = taskItemService.GetById(id);
            if (taskitem == null)
            {
                return HttpNotFound();
            }
            ViewBag.TaskId = new SelectList(taskService.GetAll(), "ID", "Name", taskitem.TaskId);
            ViewBag.PageCategory = new SelectList(DictionaryDataUtil.GetData("pageType"), "ID", "Name", taskitem.PageCategory);
            
            return View(taskitem);
            
        }

        //
        // POST: /TaskItem/Edit/5

        [HttpPost]
        public ActionResult Edit(Crawler.Entity.TaskItem taskitem)
        {
            if (ModelState.IsValid)
            {
                taskItemService.Context.Entry<TaskItem>(taskitem).State = System.Data.EntityState.Modified;
                //db.Entry(taskitem).State = EntityState.Modified;
                taskItemService.Context.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TaskId = new SelectList(taskService.GetAll(), "ID", "Name", taskitem.TaskId);
            ViewBag.PageCategory = new SelectList(DictionaryDataUtil.GetData("pageType"), "ID", "Name", 1);
            return View(taskitem);
            //try
            //{
            //    // TODO: Add update logic here

            //    return RedirectToAction("Index");
            //}
            //catch
            //{
            //    return View();
            //}
        }

        //
        // GET: /TaskItem/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /TaskItem/Delete/5

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
