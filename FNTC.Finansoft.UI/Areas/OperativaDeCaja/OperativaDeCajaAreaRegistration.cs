using System.Web.Mvc;

namespace FNTC.Finansoft.UI.Areas.OperativaDeCaja
{
    public class OperativaDeCajaAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "OperativaDeCaja";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "OperativaDeCaja_default",
                "OperativaDeCaja/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}