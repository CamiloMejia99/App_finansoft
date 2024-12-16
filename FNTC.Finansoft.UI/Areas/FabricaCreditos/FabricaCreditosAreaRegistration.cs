using System.Web.Mvc;

namespace FNTC.Finansoft.UI.Areas.FabricaCreditos
{
    public class FabricaCreditosAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "FabricaCreditos";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "FabricaCreditos_default",
                "FabricaCreditos/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}