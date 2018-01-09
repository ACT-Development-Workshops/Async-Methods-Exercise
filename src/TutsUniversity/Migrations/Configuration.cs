using System.Data.Entity.Migrations;
using TutsUniversity.DAL;

namespace TutsUniversity.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<SchoolContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }
    }
}