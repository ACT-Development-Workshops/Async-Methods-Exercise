using System;

namespace TutsUniversity.Models.Repositories
{
    public interface IUpdateRepository : IDisposable
    {
        void Add(Update update);
    }
}