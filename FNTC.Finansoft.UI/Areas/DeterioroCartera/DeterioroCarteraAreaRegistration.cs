using System.Web.Mvc;

namespace FNTC.Finansoft.UI.Areas.DeterioroCartera
{
    public class DeterioroCarteraAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "DeterioroCartera";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "DeterioroCartera_default",
                "DeterioroCartera/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}