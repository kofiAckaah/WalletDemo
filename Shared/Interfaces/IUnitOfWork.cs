using Domain.Entities;
using System.Threading.Tasks;
using System.Threading;
using System;

namespace Shared.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        Task<int> Commit(CancellationToken cancellationToken);
        IRepositoryAsync<T> Repository<T>() where T : class, IEntity;
        Task RollBack();
    }
}
