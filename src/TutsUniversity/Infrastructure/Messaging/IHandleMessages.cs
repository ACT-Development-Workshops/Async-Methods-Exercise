using System;
using System.Collections.Generic;
using TutsUniversity.Models.Commands;

namespace TutsUniversity.Infrastructure.Messaging
{
    public interface IHandleMessages<TMessage>
    {
        void Handle(TMessage message);
    }
}