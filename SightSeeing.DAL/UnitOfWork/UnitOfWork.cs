using SightSeeing.Abstraction.Interfaces;
using SightSeeing.DAL.DbContext;
using SightSeeing.DAL.Repositories;
using SightSeeing.Entities.Entities;

namespace SightSeeing.DAL.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SightSeeingDbContext _context;
        private IRepository<User> _users;
        private IRepository<Place> _places;
        private IRepository<Review> _reviews;
        private IRepository<Question> _questions;
        private IRepository<Answer> _answers;
        private IRepository<AdditionalInfo> _additionalInfos;

        public UnitOfWork(SightSeeingDbContext context)
        {
            _context = context;
        }

        public IRepository<User> Users => _users ??= new Repository<User>(_context);
        public IRepository<Place> Places => _places ??= new Repository<Place>(_context);
        public IRepository<Review> Reviews => _reviews ??= new Repository<Review>(_context);
        public IRepository<Question> Questions => _questions ??= new Repository<Question>(_context);
        public IRepository<Answer> Answers => _answers ??= new Repository<Answer>(_context);
        public IRepository<AdditionalInfo> AdditionalInfos => _additionalInfos ??= new Repository<AdditionalInfo>(_context);

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}