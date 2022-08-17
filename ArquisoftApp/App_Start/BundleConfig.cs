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

            //CSS Files
            bundles.Add(new ScriptBundle("~/bundles/Plugins/css").Include(
            "~/Content/Plugins/datatable/css/jquery.dataTables.min.css",
            "~/Content/Plugins/datatable/css/responsive.dataTables.min.css",
            "~/Content/Plugins/datatable/css/buttons.dataTables.min.css",
            "~/Content/Plugins/fontawesome-free/css/all.min.css",
            "~/Content/Plugins/sweetalert2-theme-bootstrap-4/bootstrap-4.min.css",
            "~/Content/Plugins/toastr/toastr.min.css",
            "~/Content/Plugins/admin-lte-theme/css/adminlte.css"
            ));

            // JS Files
            bundles.Add(new ScriptBundle("~/bundles/Plugins/js").Include(
            "~/Content/Plugins/datatable/js/jquery.dataTables.min.js",
            "~/Content/Plugins/datatable/js/dataTables.responsive.min.js",
            "~/Content/Plugins/datatable/js/dataTables.buttons.min.js"
            ));

            // Arquisoft Files 
            bundles.Add(new ScriptBundle("~/bundles/App/js").Include(
            "~/Scripts/components/app.js",
            "~/Scripts/components/api.js"
            ));

        }
    }
}
