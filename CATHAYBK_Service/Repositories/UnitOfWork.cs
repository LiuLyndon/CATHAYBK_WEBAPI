using BasicEIP_Core.Repositories;
using CATHAYBK_Service.DatabseContext;

namespace CATHAYBK_Service.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public IRepository<T> Repository<T>() where T : class => new Repository<T>(_context);

        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
    }
}

