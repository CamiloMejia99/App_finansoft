using System.Web.Mvc;

namespace FNTC.Finansoft.UI.Areas.EstadoCuenta
{
    public class EstadoCuentaAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "EstadoCuenta";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "EstadoCuenta_default",
                "EstadoCuenta/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}