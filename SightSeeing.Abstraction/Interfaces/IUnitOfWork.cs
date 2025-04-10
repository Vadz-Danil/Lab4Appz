using SightSeeing.Entities;
using System;
using System.Threading.Tasks;

namespace SightSeeing.Abstraction.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<User> Users { get; }
        IRepository<Place> Places { get; }
        IRepository<Review> Reviews { get; }
        IRepository<Question> Questions { get; }
        IRepository<Answer> Answers { get; }
        IRepository<AdditionalInfo> AdditionalInfos { get; }
        Task<int> SaveChangesAsync();
    }
}