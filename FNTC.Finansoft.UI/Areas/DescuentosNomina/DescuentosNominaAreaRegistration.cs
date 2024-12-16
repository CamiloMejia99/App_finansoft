using System.Web.Mvc;

namespace FNTC.Finansoft.UI.Areas.DescuentosNomina
{
    public class DescuentosNominaAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "DescuentosNomina";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "DescuentosNomina_default",
                "DescuentosNomina/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}