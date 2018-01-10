using System.Web.Mvc;
using TutsUniversity.Infrastructure.Auditing;

namespace TutsUniversity
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new AuditUpdatesFilter());
        }
    }
}