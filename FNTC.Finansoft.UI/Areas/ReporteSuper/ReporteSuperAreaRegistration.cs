using System.Web.Mvc;

namespace FNTC.Finansoft.UI.Areas.ReporteSuper
{
    public class ReporteSuperAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "ReporteSuper";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "ReporteSuper_default",
                "ReporteSuper/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}