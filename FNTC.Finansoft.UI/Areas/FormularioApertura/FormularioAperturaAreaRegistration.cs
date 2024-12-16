using System.Web.Mvc;

namespace FNTC.Finansoft.UI.Areas.FormularioApertura
{
    public class FormularioAperturaAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "FormularioApertura";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "FormularioApertura_default",
                "FormularioApertura/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}