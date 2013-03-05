using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crawler.Entity;
using Husb.Data;

namespace Crawler.Service
{
    public interface ITaskService : IRepository<Task>
    {

    }

    public class TaskService : ServiceBase<Task>, ITaskService
    {
        private readonly IRepository<Task> taskRepository;

        public TaskService(IRepository<Task> taskRepository)
            : base(taskRepository)
        {
            this.taskRepository = taskRepository;

        }

    }
}

