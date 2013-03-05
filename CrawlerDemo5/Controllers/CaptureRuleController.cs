using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Crawler.Entity;
using Crawler.Service;
using CrawlerDemo5.ViewModels;
using Husb.Common;

namespace CrawlerDemo5.Controllers
{
    public class CaptureRuleController : Controller
    {
        private readonly ICaptureRuleService captureRuleService;
        private readonly ITaskItemService taskItemService;

        public CaptureRuleController(ITaskItemService taskItemService,
            ICaptureRuleService captureRuleService)
        {
            this.captureRuleService = captureRuleService;
            this.taskItemService = taskItemService;

        }
        [HttpPost]
        public JsonResult GetPaged(int pageIndex, string taskitemid, FormCollection forms)
        {
            IEnumerable<Crawler.Entity.CaptureRule> tasks = String.IsNullOrEmpty(taskitemid) ? captureRuleService.GetAll(): captureRuleService.GetMany(c => c.TaskItems.Any(t => t.ID == new Guid(taskitemid)));
            EasyUIDataGridModel<CaptureRuleViewModel> tms = new EasyUIDataGridModel<CaptureRuleViewModel>();
            tms.rows = tasks.Select(t => new CaptureRuleViewModel
            {
                ID = t.ID,
                Name = t.Name,
                Category = t.Category,
                ContentType = t.ContentType,
                CreatedDate = t.CreatedDate,
                EndString = t.EndString,
                MatchMethod = t.MatchMethod,
                MatchRegex = t.MatchRegex,
                MaxPageCount = t.MaxPageCount,
                NextPage = t.NextPageText,
                NextPageUrl = t.NextPageUrlXPath,
                Prefix = t.Prefix,
                ProcessMethod = t.ProcessMethod,
                ReplaceRegex = t.ReplaceRegex,
                RowStatus = t.RowStatus,
                SortOrder = t.SortOrder,
                StartString = t.StartString,
                Suffix = t.Suffix,
                XPath = t.XPath


            });
            // TaskViewModel
            tms.total = 100;

            JsonResult json = Json(tms, JsonRequestBehavior.AllowGet);
            return json;
        }

        //
        // GET: /CaptureRule/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /CaptureRule/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /CaptureRule/Create

        public ActionResult Create(string taskitemid)
        {
            SetData(taskitemid);
            return View();
        }

        private void SetData(string taskitemid)
        {
            ViewBag.TaskItemId = taskitemid;
            ViewBag.CaptureRuleId = new SelectList(captureRuleService.GetAll(), "ID", "Name");
            ViewBag.PageCategory = new SelectList(DictionaryDataUtil.GetData("pageType"), "ID", "Name", 1);
            ViewBag.ContentType = new SelectList(DictionaryDataUtil.GetData("contentType"), "ID", "Name", 2);
            ViewBag.Category = new SelectList(DictionaryDataUtil.GetData("captureRuleType"), "ID", "Name", 2);
            ViewBag.MatchMethod = new SelectList(DictionaryDataUtil.GetData("matchMethod"), "ID", "Name", 1);
            ViewBag.ProcessMethod = new SelectList(DictionaryDataUtil.GetData("processMethod"), "ID", "Name", 1);
        }

        //
        // POST: /CaptureRule/Create

        [HttpPost, ValidateInput(false)]
        public ActionResult Create(string taskitemid, string captureRuleId, CaptureRule captureRule)
        {
            TaskItem item = new TaskItem { ID = new Guid(taskitemid) };
            taskItemService.Context.Set<TaskItem>().Attach(item);

            if (!String.IsNullOrEmpty(captureRuleId))
            {
                ////TaskItem item = new TaskItem { ID = new Guid( taskitemid)};
                ////TaskItem item = rule.TaskItems.SingleOrDefault(i => i.ID == new Guid(taskitemid));

                //CaptureRule rule = new CaptureRule {ID = new Guid( captureRuleId)};
                //rule.TaskItems = new HashSet<TaskItem> { new TaskItem { ID = new Guid( taskitemid)} };
                //captureRuleService.Context.Set<CaptureRule, Guid>().Attach(rule);
                //captureRuleService.Context.Entry<CaptureRule, Guid>().State = System.Data.EntityState.



                //CaptureRule rule = item.CaptureRules.SingleOrDefault(c => c.ID == new Guid(captureRuleId));
                captureRule = new CaptureRule { ID = new Guid(captureRuleId) };
                captureRuleService.Context.Set<CaptureRule>().Attach(captureRule);
            }
            else
            {
                captureRule.ID = Guid.NewGuid();
            }
            if (ModelState.IsValid)
            {
                
                item.CaptureRules.Add(captureRule);

                //captureRuleService.Create(rule);
                taskItemService.Context.SaveChanges();
                return RedirectToAction("Index");
            }
            //try
            //{
            //    // TODO: Add insert logic here

            //    return RedirectToAction("Index");
            //}
            //catch
            //{
            //    return View();
            //}

            SetData(taskitemid);
            return View();
        }

        //
        // GET: /CaptureRule/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /CaptureRule/Edit/5

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
        // GET: /CaptureRule/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /CaptureRule/Delete/5

        [HttpPost]
        public ActionResult Delete(string id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                captureRuleService.Delete(captureRuleService.GetById(new Guid(id)));
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
