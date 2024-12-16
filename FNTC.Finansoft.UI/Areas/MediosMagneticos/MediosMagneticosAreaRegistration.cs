using System.Web.Mvc;

namespace FNTC.Finansoft.UI.Areas.MediosMagneticos
{
    public class MediosMagneticosAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "MediosMagneticos";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "MediosMagneticos_default",
                "MediosMagneticos/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}