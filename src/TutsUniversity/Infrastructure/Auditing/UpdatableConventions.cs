using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Reflection;
using TutsUniversity.Infrastructure.Reflection;

namespace TutsUniversity.Infrastructure.Auditing
{
    public class UpdatableConventions : Convention
    {
        public UpdatableConventions()
        {
            Types()
                .Having(type =>
                {
                    if (!type.IsClass || !typeof(IUpdatable).IsAssignableFrom(type))
                        return null;

                    var updateIdProperty = type
                        .AllCustomClassesInHierarchy()
                        .SelectMany(t => t.GetProperties(BindingFlags.NonPublic | BindingFlags.Instance))
                        .Single(p => p.Name == "UpdateId" && p.PropertyType == typeof(int?) && p.CanRead && p.CanWrite);

                    return (updateIdProperty.ReflectedType == type) ? updateIdProperty : null;
                })
                .Configure((configuration, updateIdProperty) => configuration.Property(updateIdProperty).IsOptional());
        }
    }
}