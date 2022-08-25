using System.Web;
using System.Web.Optimization;

namespace ArquisoftApp
{
    public class BundleConfig
    {
        // Para obtener más información sobre las uniones, visite https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            // JQuery
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));
            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Modernizr
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            //Bootstrap
            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));
            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            //Plugins
            bundles.Add(new StyleBundle("~/bundles/Plugins/css").Include(
            "~/Scripts/plugins/datatable/css/jquery.dataTables.min.css",
            "~/Scripts/plugins/datatable/css/responsive.dataTables.min.css",
            "~/Scripts/plugins/datatable/css/buttons.dataTables.min.css",
            //"~/Scripts/plugins/fontawesome-free/css/all.min.css",
            "~/Scripts/plugins/sweetalert2-theme-bootstrap-4/bootstrap-4.min.css",
            "~/Scripts/plugins/toastr/toastr.min.css",
            "~/Scripts/plugins/admin-lte-theme/css/adminlte.css"
            ));
            //).Include("~/Scripts/plugins/fontawesome-free/css/all.min.css", new CssRewriteUrlTransform()));

            bundles.Add(new ScriptBundle("~/bundles/Plugins/js").Include(
            "~/Scripts/plugins/datatable/js/jquery.dataTables.min.js",
            "~/Scripts/plugins/datatable/js/dataTables.responsive.min.js",
            "~/Scripts/plugins/datatable/js/dataTables.buttons.min.js",
            "~/Scripts/plugins/admin-lte-theme/js/adminlte.min.js",
            "~/Scripts/plugins/toastr/toastr.min.js",
            "~/Scripts/plugins/sweetalert2/sweetalert2.min.js"
            ));

            // Arquisoft Files 
            bundles.Add(new ScriptBundle("~/bundles/App/js").Include(
            "~/Scripts/components/app.js",
            "~/Scripts/components/api.js"
            ));

        }
    }
}
