namespace TutsUniversity.Infrastructure.Messaging
{
    public interface IHandleMessages<TMessage>
    {
        void Handle(TMessage message);
    }
}