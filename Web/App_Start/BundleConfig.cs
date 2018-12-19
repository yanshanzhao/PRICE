using System.Web;
using System.Web.Optimization;

namespace  Web
{
    public class BundleConfig
    {
        // 有关 Bundling 的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/js/base").Include(
              "~/assets/js/jquery-1.8.2.min.js",
              "~/assets/js/serializeJson.js",
              "~/assets/js/base/template.js",
               "~/assets/js/layer/layer.js",
                 "~/assets/js/base/common.js"
          ));

            bundles.Add(new StyleBundle("~/bundles/css/base").Include(
                "~/assets/css/base/say-base.css"
            ));


            bundles.Add(new ScriptBundle("~/bundles/js/uc").Include(
              "~/assets/js/jquery-1.8.2.min.js",
              "~/assets/js/serializeJson.js",
              "~/assets/js/base/template.js",
               "~/assets/js/pagination/jquery.pagination.js",
                 "~/assets/js/base/common.js"
          ));

            bundles.Add(new StyleBundle("~/bundles/css/uc").Include(
                "~/assets/css/base/say-base.css",
                "~/assets/js/pagination/pagination.css",
                  "~/assets/css/base/uc.css"
            ));




        }
    }
}