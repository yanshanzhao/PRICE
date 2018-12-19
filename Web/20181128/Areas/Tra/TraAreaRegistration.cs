using System.Web.Mvc;

namespace SRM.Web.Areas.Tra
{
    public class TraAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Tra";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Tra_default",
                "Tra/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
