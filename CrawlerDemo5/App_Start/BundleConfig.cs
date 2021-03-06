﻿using System.Web;
using System.Web.Optimization;

namespace CrawlerDemo5
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js",
                        "~/Scripts/jquery.layout-{version}.js"));
            bundles.Add(new ScriptBundle("~/bundles/operamask").Include(
                        "~/Scripts/operamasks-ui.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                        "~/Content/themes/base/jquery.ui.core.css",
                        "~/Content/themes/base/jquery.ui.resizable.css",
                        "~/Content/themes/base/jquery.ui.selectable.css",
                        "~/Content/themes/base/jquery.ui.accordion.css",
                        "~/Content/themes/base/jquery.ui.autocomplete.css",
                        "~/Content/themes/base/jquery.ui.button.css",
                        "~/Content/themes/base/jquery.ui.dialog.css",
                        "~/Content/themes/base/jquery.ui.slider.css",
                        "~/Content/themes/base/jquery.ui.tabs.css",
                        "~/Content/themes/base/jquery.ui.datepicker.css",
                        "~/Content/themes/base/jquery.ui.progressbar.css",
                        "~/Content/themes/base/jquery.ui.theme.css"));

            bundles.Add(new StyleBundle("~/Content/themes/cupertino/css").Include(
                        "~/Content/themes/cupertino/jquery-ui.css",
                        "~/Content/themes/cupertino/jquery-ui.min.css",
                        "~/Content/themes/cupertino/jquery.ui.theme.css",
                        "~/Content/themes/cupertino/jquery.ui.layout.css"));

            bundles.Add(new StyleBundle("~/Content/themes/apusic/css").Include(
                        "~/Content/themes/apusic/om-apusic.css"));

            bundles.Add(new StyleBundle("~/Content/themes/easyui/css").Include(
                        "~/Content/main.css",
                        "~/Content/themes/easyui/easyui.css",
                        "~/Content/icon.css"));


        }
    }
}