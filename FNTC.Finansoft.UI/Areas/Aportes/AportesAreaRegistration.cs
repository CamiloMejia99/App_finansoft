using System.Web.Mvc;

namespace FNTC.Finansoft.Areas.Aportes
{
    public class AportesAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Aportes";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Aportes_default",
                "Aportes/{controller}/{action}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}