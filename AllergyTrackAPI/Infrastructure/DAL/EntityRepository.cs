using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure.DAL
{
    public sealed class EntityRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly AllergyTrackContext _context;

        public EntityRepository(AllergyTrackContext dbContext)
        {
            _context = dbContext;
        }

        private DbSet<TEntity> Set => _context.Set<TEntity>();

        public Task<int> SaveChangesAsync(CancellationToken token)
        {
            return _context.SaveChangesAsync(token);
        }

        public DbSet<TEntity> Query => Set;

        public async Task InsertRangeAsync(List<TEntity> entities, CancellationToken token)
        {
            await Set.AddRangeAsync(entities, token);
        }

        public async Task InsertAsync(TEntity entity, CancellationToken token)
        {
            var entry = GetEntityEntry(entity);
            if (entry.State == EntityState.Detached)
                await Set.AddAsync(entity, token);
        }

        public void Delete(TEntity entity)
        {
            var entry = GetEntityEntry(entity);
            if (entry.State == EntityState.Detached)
                Set.Attach(entity);
            Set.Remove(entity);
        }

        public void Update(TEntity entity)
        {
            var entry = GetEntityEntry(entity);
            if (entry.State == EntityState.Detached)
                Set.Attach(entity);
            entry.State = EntityState.Modified;
        }

        public EntityEntry<TEntity> GetEntityEntry(TEntity entity)
        {
            return _context.Entry(entity);
        }
    }
}
