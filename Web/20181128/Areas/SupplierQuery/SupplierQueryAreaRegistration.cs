using System.Web.Mvc;

namespace SRM.Web.Areas.SupplierQuery
{
    public class SupplierQueryAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "SupplierQuery";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "SupplierQuery_default",
                "SupplierQuery/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
