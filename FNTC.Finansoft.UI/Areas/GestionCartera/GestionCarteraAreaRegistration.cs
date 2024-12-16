using System.Web.Mvc;

namespace FNTC.Finansoft.UI.Areas.GestionCartera
{
    public class GestionCarteraAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "GestionCartera";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "GestionCartera_default",
                "GestionCartera/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}