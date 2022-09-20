using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Infrastructure.DAL
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task InsertAsync(TEntity entity, CancellationToken token);

        Task InsertRangeAsync(List<TEntity> entities, CancellationToken token);

        void Update(TEntity entity);

        void Delete(TEntity entity);

        Task<int> SaveChangesAsync(CancellationToken token);

        DbSet<TEntity> Query { get; }

        EntityEntry<TEntity> GetEntityEntry(TEntity entity);
    }
}
