using Domain.Entities;
using Shared.Interfaces;
using System.Collections;
using System.Threading.Tasks;
using System.Threading;
using System;
using DAL.DbContexts;
using System.Linq;

namespace Infrastructure.Services
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly WalletDbContext appDbContext;
        private Hashtable repos;

        public UnitOfWork(WalletDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public IRepositoryAsync<T> Repository<T>() where T : class, IEntity
        {
            repos ??= new Hashtable();
            var type = typeof(T).Name;

            if (!repos.ContainsKey(type))
            {
                var repoType = typeof(RepositoryAsync<>);
                var repoInstance = Activator.CreateInstance(repoType.MakeGenericType(typeof(T)), appDbContext);
                repos.Add(type, repoInstance);
            }

            return (IRepositoryAsync<T>)repos[type];
        }

        public async Task<int> Commit(CancellationToken cancellationToken)
        {
            return await appDbContext.SaveChangesAsync(cancellationToken);
        }

        public Task RollBack()
        {
            appDbContext.ChangeTracker.Entries()
                                      .ToList()
                                      .ForEach(x => x.ReloadAsync());
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private bool disposed;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    appDbContext.Dispose();
                }
            }
            disposed = true;
        }
    }
}
