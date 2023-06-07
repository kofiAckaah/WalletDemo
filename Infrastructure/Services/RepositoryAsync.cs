using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System;
using DAL.DbContexts;
using Shared.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class RepositoryAsync<T> : IRepositoryAsync<T> where T : class, IEntity
    {
        private readonly WalletDbContext appDbContext;

        public RepositoryAsync(WalletDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public IQueryable<T> Entities
            => appDbContext.Set<T>();

        public async Task<bool> ExistsAsync(Guid id)
        {
            var entityExist = await appDbContext.Set<T>().FindAsync(id);
            return entityExist != null && entityExist.Id != Guid.Empty;
        }

        public async Task<T> AddAsync(T entity)
        {
            await appDbContext.Set<T>().AddAsync(entity);
            return entity;
        }

        public async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken)
        {
            await appDbContext.Set<T>().AddRangeAsync(entities, cancellationToken);
            return entities;
        }

        public Task DeleteAsync(T entity)
        {
            appDbContext.Set<T>().Remove(entity);
            return Task.CompletedTask;
        }

        public Task DeleteRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken)
        {
            appDbContext.RemoveRange(entities);
            return Task.CompletedTask;
        }

        public async Task<List<T>> GetAllAsync()
            => await appDbContext.Set<T>().ToListAsync();

        public async Task<T> GetByIdAsync(Guid id)
            => await appDbContext.Set<T>().FindAsync(id);

        public Task Update(T entity)
        {
            appDbContext.Update(entity);
            return Task.CompletedTask;
        }

        public async Task UpdateAsync(T entity)
        {
            T memoryEntity;
            if (appDbContext.Set<T>().Local.Any(a => a == entity))
                memoryEntity = entity;
            else
                memoryEntity = await appDbContext.Set<T>().FindAsync(entity.Id);
            appDbContext.Entry(memoryEntity).State = EntityState.Modified;
            appDbContext.Entry(memoryEntity).CurrentValues.SetValues(entity);
        }

        public async Task UpdateRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken)
        {
            var dbEntities = await appDbContext.Set<T>().Where(e => entities.Select(a => a.Id).Contains(e.Id)).ToDictionaryAsync(d => d.Id, cancellationToken);
            foreach (var entity in entities)
            {
                if (!dbEntities.TryGetValue(entity.Id, out var dbEntity))
                    throw new InvalidOperationException($"Cannot update non-existing database entity for Id:{entity.Id}");

                appDbContext.Entry(dbEntity).CurrentValues.SetValues(entity);
            }
        }
    }
}
