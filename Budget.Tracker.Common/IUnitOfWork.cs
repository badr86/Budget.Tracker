using System;
using System.Threading;
using System.Threading.Tasks;

namespace Budget.Tracker.Common
{
    public interface IUnitOfWork : IDisposable
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}