namespace TutsUniversity.Infrastructure.Messaging
{
    public interface IBus
    {
        void Send<TMessage>(TMessage message);
    }
}