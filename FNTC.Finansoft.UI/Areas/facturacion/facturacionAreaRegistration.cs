using System.Web.Mvc;

namespace FNTC.Finansoft.UI.Areas.facturacion
{
    public class facturacionAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "facturacion";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "facturacion_default",
                "facturacion/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}