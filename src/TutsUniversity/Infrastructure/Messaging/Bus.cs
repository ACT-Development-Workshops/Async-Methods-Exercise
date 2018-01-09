using TutsUniversity.Infrastructure.Messaging.Providers;

namespace TutsUniversity.Infrastructure.Messaging
{
    public abstract class Bus
    {
        public abstract void Send<TMessage>(TMessage message);

        public static Bus Instance => new InMemoryBus();
    }
}