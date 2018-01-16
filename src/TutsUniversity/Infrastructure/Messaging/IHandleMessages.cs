using System.Threading.Tasks;

namespace TutsUniversity.Infrastructure.Messaging
{
    public interface IHandleMessages<TMessage>
    {
        Task Handle(TMessage message);
    }
}