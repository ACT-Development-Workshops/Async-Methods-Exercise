using System;
using System.Threading.Tasks;

namespace TutsUniversity.Models.Repositories
{
    public interface IUpdateRepository : IDisposable
    {
        Task Add(Update update);
    }
}