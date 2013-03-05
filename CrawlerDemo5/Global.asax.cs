using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using Husb.Web;

namespace CrawlerDemo5
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();

            var container = new UnityContainer();
            container.LoadConfiguration();

            // 如果需要 类似 ServiceLocator.Current.GetInstance<ITaskItemService>()这样的代码，
            // 则需要下面两行
            //var serviceLocator = new UnityServiceLocator(container);
            //Microsoft.Practices.ServiceLocation.ServiceLocator.SetLocatorProvider(() => serviceLocator);

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}