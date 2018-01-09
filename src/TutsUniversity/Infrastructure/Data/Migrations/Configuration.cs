using System.Data.Entity.Migrations;

namespace TutsUniversity.Infrastructure.Data.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<SchoolContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }
    }
}