using TutsUniversity.Infrastructure.Data;

namespace TutsUniversity.Models.Repositories.Providers
{
    public class UpdateRepository : IUpdateRepository
    {
        private readonly TutsUniversityContext context = new TutsUniversityContext();

        public void Add(Update update)
        {
            context.Updates.Add(update);
            context.SaveChanges();
        }

        public void Dispose() => context.Dispose();
    }
}