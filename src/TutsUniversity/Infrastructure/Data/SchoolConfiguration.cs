using System.Data.Entity;
using System.Data.Entity.SqlServer;

namespace TutsUniversity.Infrastructure.Data
{
    public class SchoolConfiguration : DbConfiguration
    {
        public SchoolConfiguration()
        {
            SetExecutionStrategy("System.Data.SqlClient", () => new SqlAzureExecutionStrategy());
        }
    }
}