using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TutsUniversity.Infrastructure.Messaging.Providers
{
    public class InMemoryBus : Bus
    {
        private static readonly Dictionary<Type, Type> HandlerByMessageType = new Dictionary<Type, Type>();

        public override async Task Send<TMessage>(TMessage message)
        {
            var handler = (IHandleMessages<TMessage>)Activator.CreateInstance(HandlerByMessageType[message.GetType()]);
            try
            {
                await ConfigurableYield().ConfigureAwait(false);//Clear context by forcing async execution w/context free continuation
                await handler.Handle(message);//Handlers will execute in context free environment
            }
            finally
            {
                (handler as IDisposable)?.Dispose();
            }
        }

        private static async Task ConfigurableYield() => await Task.Yield();//Forces Async Execution

        static InMemoryBus()
        {
            foreach (var type in typeof(InMemoryBus).Assembly.GetTypes().Where(t => t.IsClass && !t.IsAbstract))
            {
                var handleMessages = type
                    .GetInterfaces()
                    .SingleOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IHandleMessages<>));

                if (handleMessages != null)
                    HandlerByMessageType.Add(handleMessages.GetGenericArguments().Single(), type);
            }
        }
    }
}