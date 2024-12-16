using System.Web.Mvc;

namespace FNTC.Finansoft.UI.Areas.GestionDocumental
{
    public class GestionDocumentalAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "GestionDocumental";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "GestionDocumental_default",
                "GestionDocumental/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}