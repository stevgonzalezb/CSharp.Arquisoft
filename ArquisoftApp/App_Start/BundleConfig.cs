using System.Web;
using System.Web.Optimization;

namespace ArquisoftApp
{
    public class BundleConfig
    {
        // Para obtener más información sobre las uniones, visite https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            //Bundle plugins and libs

            // CSS Files
            bundles.Add(new StyleBundle("~/bundles/plugins/css").Include(
            "~/Scripts/plugins/datatable/css/jquery.dataTables.min.css", // Datatables
            "~/Scripts/plugins/datatable/css/responsive.dataTables.min.css",
            "~/Scripts/plugins/datatable/css/buttons.dataTables.min.css",
            "~/Scripts/plugins/sweetalert2-theme-bootstrap-4/bootstrap-4.min.css", // Sweetalert
            "~/Scripts/plugins/toastr/toastr.min.css", // Toastr
            "~/Scripts/plugins/admin-lte-theme/css/adminlte.css", // Admin LTE Theme
            "~/Scripts/plugins/bootstrap/bootstrap.css", // Bootstrap
            "~/Scripts/plugins/ion-rangeslider/ion.rangeSlider.min.css", //Ion Range Slider
            "~/Scripts/plugins/bootstrap-slider/bootstrap-slider.min.css", // Bootstrap Slider
            "~/Scripts/plugins/select2/select2-bootstrap4.css", // Select2
            "~/Scripts/plugins/select2/select2.min.css",
            "~/Scripts/plugins/dropzone/dropzone.min.css", // Dropzone
            "~/Scripts/plugins/daterangepicker/daterangepicker.css", // Date Range Picker
            "~/Content/site.css"
            ));

            // JS Files
            bundles.Add(new ScriptBundle("~/bundles/plugins/js").Include(
            "~/Scripts/plugins/datatable/js/jquery.dataTables.min.js", // Datatables
            "~/Scripts/plugins/datatable/js/dataTables.responsive.min.js",
            "~/Scripts/plugins/datatable/js/dataTables.buttons.min.js",
            "~/Scripts/plugins/sweetalert2/sweetalert2.min.js", // Sweetalert
            "~/Scripts/plugins/toastr/toastr.min.js", // Toastr
            "~/Scripts/plugins/admin-lte-theme/js/adminlte.min.js", // Admin LTE Theme
            "~/Scripts/plugins/jquery/jquery.validate*", // JQuery
            "~/Scripts/plugins/jquery/jquery-{version}.js",
            "~/Scripts/plugins/modernizr/modernizr-*", // Modernizr
            "~/Scripts/plugins/bootstrap/bootstrap.js", // Bootstrap
            "~/Scripts/plugins/bootstrap-switch/bootstrap-switch.min.js"
            ));

            // Arquisoft Files 
            bundles.Add(new ScriptBundle("~/bundles/App/js").Include(
            "~/Scripts/components/app.js",
            "~/Scripts/components/api.js"
            ));

        }
    }
}
