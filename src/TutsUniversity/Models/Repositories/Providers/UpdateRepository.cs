using System.Threading.Tasks;
using TutsUniversity.Infrastructure.Data;

namespace TutsUniversity.Models.Repositories.Providers
{
    public class UpdateRepository : IUpdateRepository
    {
        private readonly TutsUniversityContext context = new TutsUniversityContext();

        public Task Add(Update update)
        {
            context.Updates.Add(update);
            return context.SaveChangesAsync();
        }

        public void Dispose() => context.Dispose();
    }
}