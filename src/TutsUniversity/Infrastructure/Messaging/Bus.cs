using System.Threading.Tasks;
using TutsUniversity.Infrastructure.Messaging.Providers;

namespace TutsUniversity.Infrastructure.Messaging
{
    public abstract class Bus
    {
        public abstract Task Send<TMessage>(TMessage message);

        public static Bus Instance => new InMemoryBus();
    }
}