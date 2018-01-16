using System;
using System.Web.Mvc;
using TutsUniversity.Models;
using TutsUniversity.Models.Repositories;

namespace TutsUniversity.Infrastructure.Auditing
{
    public class AuditUpdatesFilter : ActionFilterAttribute
    {
        private readonly IUpdateRepository updateRepository = RepositoryFactory.Updates;

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Request.HttpMethod != "POST")
                return;
            if (filterContext.ActionDescriptor.ActionName.StartsWith("Create", StringComparison.InvariantCultureIgnoreCase))
                return;

            var update = new Update { MadeBy = Environment.UserName, MadeOnUtc = DateTime.UtcNow };
            updateRepository.Add(update).GetAwaiter().GetResult();

            UpdateContext.CurrentUpdateId = update.Id;
        }
    }
}