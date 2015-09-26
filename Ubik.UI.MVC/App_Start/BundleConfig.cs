using System.Web.Optimization;

namespace Ubik.UI.MVC
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                    "~/Areas/Backoffice/Scripts/bootstrap.min.js"));
            
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                    "~/Areas/Backoffice/Scripts/jquery.min.js"));
            
            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                    "~/Areas/Backoffice/Scripts/jquery-ui.min.js"));
            
            bundles.Add(new ScriptBundle("~/bundles/raphael").Include(
                    "~/Areas/Backoffice/Scripts/raphael-min.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap-slider").Include(
                    "~/Areas/Backoffice/Scripts/plugins/bootstrap-slider/bootstrap-slider.js"));
            
            bundles.Add(new ScriptBundle("~/bundles/bootstrap-wysihtml5").Include(
                    "~/Areas/Backoffice/Scripts/plugins/bootstrap-wysihtml5/bootstrap3-wysihtml5.all.min.js"));
            
            bundles.Add(new ScriptBundle("~/bundles/colorpicker").Include(
                    "~/Areas/Backoffice/Scripts/plugins/colorpicker/bootstrap-colorpicker.min.js"));
            
            bundles.Add(new ScriptBundle("~/bundles/datatables").Include(
                    "~/Areas/Backoffice/Scripts/plugins/datatables/jquery.dataTables.js",
                    "~/Areas/Backoffice/Scripts/plugins/datatables/dataTables.bootstrap.js"));
            
            bundles.Add(new ScriptBundle("~/bundles/datepicker").Include(
                    "~/Areas/Backoffice/Scripts/plugins/datepicker/bootstrap-datepicker.js"));
            
            bundles.Add(new ScriptBundle("~/bundles/daterangepicker").Include(
                    "~/Areas/Backoffice/Scripts/plugins/daterangepicker/daterangepicker.js"));
            
            bundles.Add(new ScriptBundle("~/bundles/flot").Include(
                    "~/Areas/Backoffice/Scripts/plugins/flot/jquery.flot.min.js",
                    "~/Areas/Backoffice/Scripts/plugins/flot/jquery.flot.resize.min.js",
                    "~/Areas/Backoffice/Scripts/plugins/flot/jquery.flot.pie.min.js",
                    "~/Areas/Backoffice/Scripts/plugins/flot/jquery.flot.categories.min.js"));
            
            bundles.Add(new ScriptBundle("~/bundles/iCheck").Include(
                    "~/Areas/Backoffice/Scripts/plugins/iCheck/icheck.min.js"));
            
            bundles.Add(new ScriptBundle("~/bundles/input-mask").Include(
                    "~/Areas/Backoffice/Scripts/plugins/input-mask/jquery.inputmask.js",
                    "~/Areas/Backoffice/Scripts/plugins/input-mask/jquery.inputmask.date.extensions.js",
                    "~/Areas/Backoffice/Scripts/plugins/input-mask/jquery.inputmask.extensions.js"));
            
            bundles.Add(new ScriptBundle("~/bundles/ionslider").Include(
                    "~/Areas/Backoffice/Scripts/plugins/ionslider/ion.rangeSlider.min.js"));
            
            bundles.Add(new ScriptBundle("~/bundles/jqueryKnob").Include(
                    "~/Areas/Backoffice/Scripts/plugins/jqueryKnob/jquery.knob.js"));
            
            bundles.Add(new ScriptBundle("~/bundles/jvectormap").Include(
                    "~/Areas/Backoffice/Scripts/plugins/jvectormap/jquery-jvectormap-1.2.2.min.js",
                    "~/Areas/Backoffice/Scripts/plugins/jvectormap/jquery-jvectormap-world-mill-en.js"));
            
            bundles.Add(new ScriptBundle("~/bundles/misc").Include(
                    "~/Areas/Backoffice/Scripts/plugins/misc/html5shiv.js",
                    "~/Areas/Backoffice/Scripts/plugins/misc/jquery.ba-resize.min.js",
                    "~/Areas/Backoffice/Scripts/plugins/misc/jquery.placeholder.js",
                    "~/Areas/Backoffice/Scripts/plugins/misc/modernizr.min.js",
                    "~/Areas/Backoffice/Scripts/plugins/misc/respond.min.js"));
            
            bundles.Add(new ScriptBundle("~/bundles/morris").Include(
                    "~/Areas/Backoffice/Scripts/plugins/morris/morris.min.js"));
            
            bundles.Add(new ScriptBundle("~/bundles/slimScroll").Include(
                    "~/Areas/Backoffice/Scripts/plugins/slimScroll/jquery.slimscroll.min.js"));
            
            bundles.Add(new ScriptBundle("~/bundles/sparkline").Include(
                    "~/Areas/Backoffice/Scripts/plugins/sparkline/jquery.sparkline.min.js"));
            
            bundles.Add(new ScriptBundle("~/bundles/timepicker").Include(
                    "~/Areas/Backoffice/Scripts/plugins/timepicker/bootstrap-timepicker.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/AdminLTE").Include(
                    "~/Areas/Backoffice/Scripts/AdminLTE/app.js",
                    "~/Areas/Backoffice/Scripts/AdminLTE/demo.js"));

            bundles.Add(new ScriptBundle("~/bundles/AdminLTE_Dashboard").Include(
               "~/Areas/Backoffice/Scripts/AdminLTE/dashboard.js"));

            bundles.Add(new ScriptBundle("~/bundles/Calendar").Include(
               "~/Areas/Backoffice/Scripts/plugins/calendar/moment.min.js",
               "~/Areas/Backoffice/Scripts/plugins/calendar/fullcalendar.min.js"));


            bundles.Add(new StyleBundle("~/Areas/Backoffice/Content/css/AdminLTE").Include(
                      "~/Areas/Backoffice/Content/css/AdminLTE.css",
                      "~/Areas/Backoffice/Content/css/AdminLTE_Overrides.css"));    

            bundles.Add(new StyleBundle("~/Areas/Backoffice/Content/css/bootstrap").Include(
                      "~/Areas/Backoffice/Content/css/bootstrap.min.css"));

            bundles.Add(new StyleBundle("~/Areas/Backoffice/Content/css/bootstrap-slider").Include(
                    "~/Areas/Backoffice/Content/css/bootstrap-slider/slider.css"));
            
            bundles.Add(new StyleBundle("~/Areas/Backoffice/Content/css/bootstrap-wysihtml5").Include(
                      "~/Areas/Backoffice/Content/css/bootstrap-wysihtml5/bootstrap3-wysihtml5.min.css"));
            
            bundles.Add(new StyleBundle("~/Areas/Backoffice/Content/css/colorpicker").Include(
                   "~/Areas/Backoffice/Content/css/colorpicker/bootstrap-colorpicker.min.css"));
            
            bundles.Add(new StyleBundle("~/Areas/Backoffice/Content/css/datatables").Include(
                      "~/Areas/Backoffice/Content/css/datatables/dataTables.bootstrap.css"));
            
            bundles.Add(new StyleBundle("~/Areas/Backoffice/Content/css/datepicker").Include(
                   "~/Areas/Backoffice/Content/css/datepicker/datepicker3.css"));
            
            bundles.Add(new StyleBundle("~/Areas/Backoffice/Content/css/daterangepicker").Include(
                      "~/Areas/Backoffice/Content/css/daterangepicker/daterangepicker-bs3.css"));
            
            bundles.Add(new StyleBundle("~/Areas/Backoffice/Content/css/iCheck").Include(
                   "~/Areas/Backoffice/Content/css/iCheck/all.css"));
            
            bundles.Add(new StyleBundle("~/Areas/Backoffice/Content/css/ionslider").Include(
                   "~/Areas/Backoffice/Content/css/ionslider/ion.rangeSlider.css",
                   "~/Areas/Backoffice/Content/css/ionslider/ion.rangeSlider.skinNice.css"));
            
            bundles.Add(new StyleBundle("~/Areas/Backoffice/Content/css/jvectormap").Include(
                      "~/Areas/Backoffice/Content/css/jvectormap/jquery-jvectormap-1.2.2.css"));
            
            bundles.Add(new StyleBundle("~/Areas/Backoffice/Content/css/morris").Include(
                   "~/Areas/Backoffice/Content/css/morris/morris.css"));
            
            bundles.Add(new StyleBundle("~/Areas/Backoffice/Content/css/timepicker").Include(
                      "~/Areas/Backoffice/Content/css/timepicker/bootstrap-timepicker.min.css"));

            bundles.Add(new StyleBundle("~/Areas/Backoffice/Content/css/calendar").Include(
                     "~/Areas/Backoffice/Content/css/plugins/calendar/fullCalendar.css",
                     "~/Areas/Backoffice/Content/css/plugins/calendar/fullCalendarPrint.css"));
             
        }
    }
}
