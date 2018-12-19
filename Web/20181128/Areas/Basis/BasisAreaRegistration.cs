using System.Web.Mvc;

namespace SRM.Web.Areas.Basis
{
    public class BasisAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Basis";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Basis_default",
                "Basis/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
