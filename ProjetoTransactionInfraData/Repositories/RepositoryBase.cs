using Microsoft.EntityFrameworkCore;
using ProjetoTransactionDomain.Repositories;
using ProjetoTransactionInfraData.Context;

namespace ProjetoTransactionInfraData.Repositories
{
    public class RepositoryBase<TEntity> : IRepositoryBase<TEntity> where TEntity : class
    {
        private readonly ApplicationDbContext _db;
        private readonly DbSet<TEntity> _dbSet;

        public RepositoryBase(ApplicationDbContext db)
        {
            _db = db;
            _dbSet = _db.Set<TEntity>();
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            var result = await _dbSet.AddAsync(entity);
            return result.Entity;
        }

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            var result = await Task.Run(() => _db.Update(entity));
            return result.Entity;
        }

        public async Task<TEntity?> GetByIdAsync(Guid id)
            => await _dbSet.FindAsync(id);

        public async Task<int> SaveChangesAsync()
            => await _db.SaveChangesAsync();
    }
}
