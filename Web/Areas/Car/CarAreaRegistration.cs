using System.Web.Mvc;

namespace Web.Areas.Car
{
    public class PriceAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Car";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Car_default",
                "Car/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
