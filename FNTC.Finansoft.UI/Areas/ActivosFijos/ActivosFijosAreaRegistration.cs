using System.Web.Mvc;

namespace FNTC.Finansoft.UI.Areas.ActivosFijos
{
    public class ActivosFijosAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "ActivosFijos";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "ActivosFijos_default",
                "ActivosFijos/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}