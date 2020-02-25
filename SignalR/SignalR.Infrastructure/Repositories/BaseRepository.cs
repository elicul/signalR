using Microsoft.EntityFrameworkCore;
using SignalR.Contracts.Entities;
using SignalR.Contracts.Interfaces.Infrastructure;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SignalR.Infrastructure.Repositories
{
    public abstract class BaseRepository<TDbContext, TEntity> : IBaseRepository<TEntity> where TDbContext : SQLiteDbContext where TEntity : class, new()
    {
        protected readonly DbSet<TEntity> dbSet;
        private readonly TDbContext context;

        protected BaseRepository(TDbContext context)
        {
            this.context = context;
            dbSet = context.Set<TEntity>();
        }

        public async Task AddAsync(TEntity entity)
        {
            if(entity is BaseEntity)
            {
                BaseEntity newEntity = entity as BaseEntity;

                var currentUtcTime = DateTime.UtcNow;
                newEntity.CreatedDateUtc = currentUtcTime;
                newEntity.LastModifiedDateUtc = currentUtcTime;
            }
            dbSet.Add(entity);
            await context.SaveChangesAsync();
        }

        public IQueryable<TEntity> GetAll()
        {
            return dbSet;
        }

        public TEntity GetByKey(params object[] keyValues)
        {
            return dbSet.Find(keyValues);
        }

        public async Task<TEntity> GetByKeyAsync(params object[] keyValues)
        {
            return await dbSet.FindAsync(keyValues);
        }

        public void Remove(TEntity entity)
        {
            dbSet.Remove(entity);
        }

        public void SetOriginalValues(TEntity entity)
        {
            context.Entry(entity).OriginalValues.SetValues((object)entity);
        }

        public async Task UpdateAsync(TEntity entity)
        {
            if (entity is BaseEntity)
            {
                BaseEntity newEntity = entity as BaseEntity;

                var currentUtcTime = DateTime.UtcNow;
                newEntity.LastModifiedDateUtc = currentUtcTime;
            }
            context.Entry(entity).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }
    }
}
