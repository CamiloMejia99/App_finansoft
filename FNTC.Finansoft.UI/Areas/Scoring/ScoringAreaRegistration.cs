using System.Web.Mvc;

namespace FNTC.Finansoft.UI.Areas.Scoring
{
    public class ScoringAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Scoring";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Scoring_default",
                "Scoring/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}