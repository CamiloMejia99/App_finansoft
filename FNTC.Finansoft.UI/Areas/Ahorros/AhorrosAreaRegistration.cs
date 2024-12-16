using System.Web.Mvc;

namespace FNTC.Finansoft.Areas.Ahorros
{
    public class AhorrosAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Ahorros";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Ahorros_default",
                "Ahorros/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}