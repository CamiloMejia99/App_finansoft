using System.Web.Mvc;

namespace FNTC.Finansoft.UI.Areas.PruebaEstructuraCapas
{
    public class PruebaEstructuraCapasAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "PruebaEstructuraCapas";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "PruebaEstructuraCapas_default",
                "PruebaEstructuraCapas/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}