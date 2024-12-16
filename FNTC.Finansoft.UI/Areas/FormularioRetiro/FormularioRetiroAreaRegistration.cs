using System.Web.Mvc;

namespace FNTC.Finansoft.UI.Areas.FormularioRetiro
{
    public class FormularioRetiroAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "FormularioRetiro";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "FormularioRetiro_default",
                "FormularioRetiro/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}