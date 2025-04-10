using System.Collections.Generic;
using System.Threading.Tasks;

namespace SightSeeing.Abstraction.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<T> GetByIdAsync(int id, bool eager = false);
        Task<IEnumerable<T>> GetAllAsync(bool eager = false);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(int id);
    }
}