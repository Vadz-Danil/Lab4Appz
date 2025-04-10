using Microsoft.EntityFrameworkCore;
using SightSeeing.DAL.DbContext;
using System.Collections.Generic;
using System.Threading.Tasks;
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
            if (eager)
            {
                return await _dbSet.ToListAsync();
            }
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
        
        private IQueryable<T> IncludeAll()
        {
            IQueryable<T> query = _dbSet;
            var entityType = typeof(T).Name;

            switch (entityType)
            {
                case nameof(Entities.User):
                    query = query.Include("Reviews").Include("Questions").Include("Answers");
                    break;
                case nameof(Entities.Place):
                    query = query.Include("Reviews").Include("Questions").Include("AdditionalInfos");
                    break;
                case nameof(Entities.Review):
                    query = query.Include("User").Include("Place");
                    break;
                case nameof(Entities.Question):
                    query = query.Include("User").Include("Place").Include("Answers");
                    break;
                case nameof(Entities.Answer):
                    query = query.Include("User").Include("Question");
                    break;
                case nameof(Entities.AdditionalInfo):
                    query = query.Include("Place");
                    break;
            }
            return query;
        }
    }
}