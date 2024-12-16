using System.Web.Mvc;

namespace FNTC.Finansoft.UI.Areas.ControlErrores
{
    public class ControlErroresAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "ControlErrores";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "ControlErrores_default",
                "ControlErrores/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}