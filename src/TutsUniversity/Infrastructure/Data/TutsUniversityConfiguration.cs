using System.Data.Entity;
using System.Data.Entity.SqlServer;

namespace TutsUniversity.Infrastructure.Data
{
    public class TutsUniversityConfiguration : DbConfiguration
    {
        public TutsUniversityConfiguration()
        {
            SetExecutionStrategy("System.Data.SqlClient", () => new SqlAzureExecutionStrategy());
        }
    }
}