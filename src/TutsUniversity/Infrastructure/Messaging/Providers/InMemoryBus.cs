using System;
using System.Collections.Generic;
using System.Linq;

namespace TutsUniversity.Infrastructure.Messaging.Providers
{
    public class InMemoryBus : IBus
    {
        private static readonly Dictionary<Type, Type> HandlerByMessageType = new Dictionary<Type, Type>();

        public void Send<TMessage>(TMessage message)
        {
            var handler = (IHandleMessages<TMessage>)Activator.CreateInstance(HandlerByMessageType[message.GetType()]);
            try
            {
                handler.Handle(message);
            }
            finally
            {
                (handler as IDisposable)?.Dispose();
            }
        }

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