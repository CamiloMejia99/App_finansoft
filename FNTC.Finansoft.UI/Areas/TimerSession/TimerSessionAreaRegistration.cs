using System.Web.Mvc;

namespace FNTC.Finansoft.UI.Areas.TimerSession
{
    public class TimerSessionAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "TimerSession";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "TimerSession_default",
                "TimerSession/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}