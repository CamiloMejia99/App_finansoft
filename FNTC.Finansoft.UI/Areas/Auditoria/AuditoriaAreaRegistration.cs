using System.Web.Mvc;

namespace FNTC.Finansoft.UI.Areas.Auditoria
{
    public class AuditoriaAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Auditoria";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Auditoria_default",
                "Auditoria/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}