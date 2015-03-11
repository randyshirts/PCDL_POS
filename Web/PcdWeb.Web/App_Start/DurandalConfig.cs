using System;
using System.Web.Optimization;
using PcdWeb.Web;

[assembly: WebActivatorEx.PostApplicationStartMethod(
    typeof(DurandalAuthDemo.App_Start.DurandalConfig), "PreStart")]

namespace DurandalAuthDemo.App_Start
{
    public static class DurandalConfig
    {
        public static void PreStart()
        {
            // Add your start logic here
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}