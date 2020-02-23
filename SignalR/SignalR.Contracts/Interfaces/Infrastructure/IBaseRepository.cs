using System.Linq;
using System.Threading.Tasks;

namespace SignalR.Contracts.Interfaces.Infrastructure
{
    public interface IBaseRepository<TEntity> where TEntity : class, new()
    {
        TEntity GetByKey(params object[] keyValues);
        Task<TEntity> GetByKeyAsync(params object[] keyValues);
        IQueryable<TEntity> GetAll();
        Task UpdateAsync(TEntity entity);
        Task AddAsync(TEntity entity);
        void Remove(TEntity entity);
        void SetOriginalValues(TEntity entity);
    }
}
