using System.Web;
using System.Web.Optimization;

namespace CarMate
{
    public class BundleConfig
    {
        // Дополнительные сведения о Bundling см. по адресу http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            /* Script */
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                "~/Scripts/jquery.unobtrusive*",
                "~/Scripts/jquery.validate*"));

            // Используйте версию Modernizr для разработчиков, чтобы учиться работать. Когда вы будете готовы перейти к работе,
            // используйте средство построения на сайте http://modernizr.com, чтобы выбрать только нужные тесты.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/script/bootstrap").Include
               ("~/js/bootstrap.min.js"));

            bundles.Add(new ScriptBundle("~/Scripts/rating").Include
               ("~/Scripts/rating/jquery.rating-2.0.js"));

            bundles.Add(new ScriptBundle("~/Script/bootstrap-datepicker").Include(
                "~/Scripts/bootstrap-datepicker/bootstrap-datepicker.js",
                "~/Scripts/bootstrap-datepicker/locales/bootstrap-datepicker.ru.js"
                //"~/Scripts/bootstrap-datepicker/bootstrap-datepicker.ru.min.js"
                ));

            bundles.Add(new ScriptBundle("~/Script/highcharts").Include(
                "~/Scripts/highcharts/js/highcharts.js",
                "~/Scripts/highcharts/js/modules/data.js",
                "~/Scripts/highcharts/js/modules/drilldown.js"));

            bundles.Add(new ScriptBundle("~/Script/landing").Include(
                "~/js/landing/jquery-1.9.1.min.js",
                "~/js/landing/jquery.placeholder.min.js",
                "~/js/landing/jquery.maskedinput.js",
                "~/js/landing/jquery.form.min.js",
                "~/js/landing/bjqs-1.3.js",
                "~/js/landing/script.js"));


            /* CSS */
            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/bootstrap").Include(
                "~/Content/bootstrap.css"));

            bundles.Add(new StyleBundle("~/Content/rating").Include(
                "~/Content/rating/jquery.rating.css"));

            bundles.Add(new StyleBundle("~/Content/bootstrap-datepicker").Include(
                "~/Content/bootstrap-datepicker/bootstrap-datepicker3.css"));

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

            bundles.Add(new StyleBundle("~/Content/landing").Include(
                "~/Content/CSS_reset_landing.css",
                "~/Content/bjqs.css",
                "~/Content/style_landing.css"));

        }
    }
}