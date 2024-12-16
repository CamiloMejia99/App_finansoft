using System.Web.Mvc;

namespace FNTC.Finansoft.UI.Areas.formularioVinculacion
{
    public class formularioVinculacionAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "formularioVinculacion";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "formularioVinculacion_default",
                "formularioVinculacion/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}