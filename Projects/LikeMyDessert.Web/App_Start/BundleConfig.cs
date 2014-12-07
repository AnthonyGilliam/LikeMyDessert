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
            bundles.Add(new ScriptBundle("~/Bundle/Scripts/jquery").Include(
                "~/Scripts/jquery/jquery-{version}.js",
                "~/Scripts/jquery/modernizr-1.7.js")
            );

            bundles.Add(new ScriptBundle("~/Bundle/Scripts/jquery-ui").Include(
                "~/Scripts/jquery/jquery-ui-1.8.18.custom.min.js")
            );
        }
    }
}