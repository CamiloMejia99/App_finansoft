using System.Web;
using System.Web.Optimization;

namespace FNTC.Finansoft.UI
{
    public class BundleConfig
    {
        // Para obtener más información sobre las uniones, visite http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*", "~/Scripts/messages_es.js"));

            // Utilice la versión de desarrollo de Modernizr para desarrollar y obtener información. De este modo, estará
            // preparado para la producción y podrá utilizar la herramienta de compilación disponible en http://modernizr.com para seleccionar solo las pruebas que necesite.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/CheckboxCss").Include(
                      "~/Content/css/checkbox.css"));

            bundles.Add(new ScriptBundle("~/bundles/Currency").Include(
                "~/Scripts/autoNumeric-min.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                      "~/Scripts/modernizr-*"));

            //Tablas

            bundles.Add(new ScriptBundle("~/Content/datatablesCss").Include(
                                    "~/Content/css/datatables.css"));

            bundles.Add(new ScriptBundle("~/bundles/datatablesJs").Include(
                                    "~/Scripts/datatables.js"));

            //swal
            bundles.Add(new ScriptBundle("~/Content/swalCss").Include(
                                    "~/Content/css/sweetalert.css"));

            bundles.Add(new ScriptBundle("~/bundles/swalJs").Include(
                                    "~/Scripts/sweetalert.min.js"));

            //otros
            bundles.Add(new ScriptBundle("~/bundles/OtrosJs").Include(
                          "~/Scripts/respond.js",
                          "~/Scripts/metisMenu.min.js",
                          "~/Scripts/sb-admin-2.js",
                          "~/Scripts/select2.js",
                          "~/Scripts/i18n/*.js",
                          "~/Scripts/mustache.min.js",
                          "~/Scripts/bootstrap-dialog.min.js",
                          "~/Scripts/jquery.tabletojson.min.js",
                          "~/Scripts/jinqjs.min.js",
                          "~/Scripts/Main.js"));

            bundles.Add(new StyleBundle("~/Content/OtrosCss").Include(
                       "~/Content/fonts/font-awesome/css/font-awesome.min.css",
                      "~/Content/css/site.css",
                      "~/Content/css/select2.css",
                      "~/Content/css/select2-bootstrap.css",
                      "~/Content/css/bootstrap-dialog.min.css"));

            bundles.Add(new ScriptBundle("~/bundles/jquery.masknumber").Include(
                        "~/Scripts/jquery.masknumber.js"));

            bundles.Add(new ScriptBundle("~/bundles/modalform").Include(
                "~/Scripts/modalform.js"));
        }
    }
}
