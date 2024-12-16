using System.Web.Mvc;

namespace FNTC.Finansoft.UI.Areas.Terceros
{
    public class TercerosAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Terceros";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Terceros_default",
                "Terceros/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}