using System;
using System.Data.Entity;
using System.Linq;
using System.Web;
using TutsUniversity.Infrastructure.Data;

namespace TutsUniversity.Infrastructure.Auditing
{
    public static class UpdateContext
    {
        static UpdateContext()
        {
            TutsUniversityContext.SavingChanges += context =>
            {
                context.ChangeTracker.DetectChanges();
                foreach (var item in context.ChangeTracker.Entries().Where(e => e.State == EntityState.Modified))
                {
                    if (item.Entity is IUpdatable updatableEntity)
                        updatableEntity.UpdateId = CurrentUpdateId.Value;//UpdateId must exist when updating an Updatable entity
                }
            };
        }

        public static int? CurrentUpdateId
        {
            get => HttpContext.Current.Items.Contains(nameof(CurrentUpdateId)) ? (int)HttpContext.Current.Items[nameof(CurrentUpdateId)] : (int?)null;
            set
            {
                if (CurrentUpdateId.HasValue)
                    throw new InvalidOperationException($"{nameof(CurrentUpdateId)} has already been set for the request");
                if (value == null)
                    return;

                HttpContext.Current.Items[nameof(CurrentUpdateId)] = value.Value;
            }
        }
    }
}