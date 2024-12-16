using System.Web.Mvc;

namespace FNTC.Finansoft.UI.Areas.LiquidacionDefinitiva
{
    public class LiquidacionDefinitivaAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "LiquidacionDefinitiva";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "LiquidacionDefinitiva_default",
                "LiquidacionDefinitiva/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}