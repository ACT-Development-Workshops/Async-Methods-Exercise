using System;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using TutsUniversity.Infrastructure.Data;

namespace TutsUniversity.Infrastructure.Auditing
{
    public static class UpdateContext
    {
        private static readonly AsyncLocal<int?> currentUpdateId = new AsyncLocal<int?>();

        static UpdateContext()
        {
            TutsUniversityContext.SavingChanges += context =>
            {
                context.ChangeTracker.DetectChanges();
                foreach (var item in context.ChangeTracker.Entries().Where(e => e.State == EntityState.Modified))
                {
                    if (item.Entity is IUpdatable updatableEntity)
                    {
                        updatableEntity.UpdateId = CurrentUpdateId.Value;//UpdateId must exist when updating an Updatable entity
                        item.Property(nameof(updatableEntity.UpdateId)).IsModified = true;
                    }
                }
            };
        }

        public static int? CurrentUpdateId
        {
            get => currentUpdateId.Value;
            set
            {
                if (CurrentUpdateId.HasValue)
                    throw new InvalidOperationException($"{nameof(CurrentUpdateId)} has already been set for the request");
                if (value == null)
                    return;

                currentUpdateId.Value = value.Value;
            }
        }
    }
}