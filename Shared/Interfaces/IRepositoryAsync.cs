using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System;
using Domain.Entities;

namespace Shared.Interfaces
{
    public interface IRepositoryAsync<T> where T : class, IEntity
    {
        IQueryable<T> Entities { get; }
        Task<T> AddAsync(T entity);
        Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken);
        Task DeleteAsync(T entity);
        Task DeleteRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken);
        Task<bool> ExistsAsync(Guid id);
        Task<List<T>> GetAllAsync();
        Task<T> GetByIdAsync(Guid id);
        Task Update(T entity);
        Task UpdateAsync(T entity);
        Task UpdateRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken);
    }
}
