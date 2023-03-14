using System.Web;
using System.Web.Optimization;

namespace Sivam.web
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            // Main Site Styles bundling
            bundles.Add(new StyleBundle("~/Site/css").Include(
                     "~/site/css/bootstrap.min.css",
                     "~/site/css/jquery-ui.min.css",
                     "~/site/font-awesome/css/font-awesome.min.css",
                     "~/site/css/flaticon.css"
                     ));

            bundles.Add(new StyleBundle("~/Site/owlcarousel").Include(
                    "~/site/owl-carousel/owl.carousel.css",
                    "~/site/owl-carousel/owl.theme.css",
                    "~/site/owl-carousel/owl.transitions.css"
                    ));

            bundles.Add(new StyleBundle("~/Site/revolution").Include(
                  "~/site/revolution/css/settings.css",
                  "~/site/revolution/css/layers.css",
                  "~/site/revolution/css/navigation.css"
                  ));

        }
    }
}
