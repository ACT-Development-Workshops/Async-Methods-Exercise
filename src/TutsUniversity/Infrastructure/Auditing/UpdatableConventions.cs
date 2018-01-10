using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Reflection;

namespace TutsUniversity.Infrastructure.Auditing
{
    public class UpdatableConventions : Convention
    {
        public UpdatableConventions()
        {
            Types()
                .Where(t => typeof(IUpdatable).IsAssignableFrom(t))
                .Configure(configuration =>
                {
                    var updateIdProperty = configuration.ClrType
                        .GetProperties(BindingFlags.NonPublic | BindingFlags.Instance)
                        .Single(p => p.Name == "UpdateId" && p.PropertyType == typeof(int?) && p.CanRead && p.CanWrite);

                    configuration.Property(updateIdProperty).IsOptional();
                });
        }
    }
}