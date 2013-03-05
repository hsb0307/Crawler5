using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Crawler.Entity;
using Husb.Data;

namespace Crawler.Service
{
    public interface ITaskCategoryService : IRepository<TaskCategory>
    {

    }

    public class TaskCategoryService : ServiceBase<TaskCategory>, ITaskCategoryService
    {
        private readonly IRepository<TaskCategory> taskCategoryRepository;

        public TaskCategoryService(IRepository<TaskCategory> taskCategoryRepository)
            : base(taskCategoryRepository)
        {
            this.taskCategoryRepository = taskCategoryRepository;

        }

    }
}
