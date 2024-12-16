using System.Web.Mvc;

namespace FNTC.Finansoft.UI.Areas.Nomina
{
    public class NominaAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Nomina";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Nomina_default",
                "Nomina/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}