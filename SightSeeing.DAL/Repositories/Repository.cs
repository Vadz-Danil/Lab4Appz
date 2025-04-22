using Microsoft.EntityFrameworkCore;
using SightSeeing.DAL.DbContext;
using SightSeeing.Abstraction.Interfaces;

namespace SightSeeing.DAL.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly SightSeeingDbContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(SightSeeingDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<T> GetByIdAsync(int id, bool eager = false)
        {
            if (eager)
            {
                return await _dbSet.SingleOrDefaultAsync(e => EF.Property<int>(e, "Id") == id);
            }
            return await _dbSet.FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAllAsync(bool eager = false)
        {
            return await _dbSet.ToListAsync();
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
            }
        }
    }
}
