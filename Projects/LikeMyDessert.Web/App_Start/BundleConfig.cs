using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace LikeMyDessert.Web.App_Start
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Bundle/Scripts/layout").Include(
                "~/Scripts/common/less-1.1.3.min.js",
                "~/Scripts/jquery/jquery-{version}.js",
                "~/Scripts/jquery/modernizr-1.7.js",
                "~/Scripts/global.js")
            );

            bundles.Add(new ScriptBundle("~/Bundle/Scripts/jquery-ui").Include(
                "~/Scripts/jquery/jquery-ui-1.8.18.custom.min.js",
                "~/Scripts/jquery/jquery.form.js",
                "~/Scripts/jquery/jquery.validate.js",
                "~/Scripts/jquery/jquery.validate.unobtrusive.js")
            );

            bundles.Add(new ScriptBundle("~/Bundle/Scripts/home-page").Include(
                "~/Scripts/homepage.js",
                "~/Scripts/dessert.js",
                "~/Scripts/topSlidePicture.js")
            );

            bundles.Add(new StyleBundle("~/Bundle/Content/css/layout").Include(
                "~/Content/css/jquery-ui-1.8.18.custom.css",
                "~/Content/css/Global.css",
                "~/Content/css/Header.css")
            );

            bundles.Add(new StyleBundle("~/Bundle/Content/css/home-page").Include(
                "~/Content/css/HomePage.css",
                "~/Content/css/AddForm.css",
                "~/Content/css/Header.css")
            );
        }
    }
}