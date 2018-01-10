using System.Data.Entity.Migrations;

namespace TutsUniversity.Infrastructure.Data.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<TutsUniversityContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"Infrastructure\Data\Migrations";
        }
    }
}