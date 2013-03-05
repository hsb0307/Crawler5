using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Crawler.Entity;
using Husb.Data;

namespace Crawler.Service
{
    public interface ICaptureRuleService : IRepository<CaptureRule>
    {

    }

    public class CaptureRuleService : ServiceBase<CaptureRule>, ICaptureRuleService
    {
        private readonly IRepository<CaptureRule> captureRuleRepository;

        public CaptureRuleService(IRepository<CaptureRule> captureRuleRepository)
            : base(captureRuleRepository)
        {
            this.captureRuleRepository = captureRuleRepository;

        }

    }
}

