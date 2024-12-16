using System.Web.Mvc;

namespace FNTC.Finansoft.UI.Areas.SIAR
{
    public class SIARAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "SIAR";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "SIAR_default",
                "SIAR/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}