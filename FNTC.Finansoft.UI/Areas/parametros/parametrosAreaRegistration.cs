using System.Web.Mvc;

namespace FNTC.Finansoft.UI.Areas.parametros
{
    public class parametrosAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "parametros";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "parametros_default",
                "parametros/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}