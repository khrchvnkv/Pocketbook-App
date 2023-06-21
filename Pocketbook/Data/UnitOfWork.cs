using Pocketbook.Core.IConfiguration;
using Pocketbook.Core.IRepositories;
using Pocketbook.Core.Repositories;

namespace Pocketbook.Data
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger _logger;

        public IUserRepository Users { get; } 

        public UnitOfWork(ApplicationDbContext context, ILoggerFactory loggerFactory)
        {
            _context = context;
            _logger = loggerFactory.CreateLogger("logs");

            Users = new UserRepository(context, _logger);
        }

        public async Task CompleteAsync() => await _context.SaveChangesAsync();
        public void Dispose() => _context.Dispose();
    }
}