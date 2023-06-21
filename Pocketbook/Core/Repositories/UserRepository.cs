using Microsoft.EntityFrameworkCore;
using Pocketbook.Core.IRepositories;
using Pocketbook.Data;
using Pocketbook.Models;

namespace Pocketbook.Core.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context, ILogger logger) : base(context, logger)
        { }

        public async Task<string?> GetFirstNameAndLastName(Guid id)
        {
            var user = await GetUserById(id);
            if (user is null) return null;

            return $"{user.FirstName} {user.LastName}".ToUpper();
        }
        protected override void CopyDataOnUpdating(User from, User to)
        {
            to.FirstName = from.FirstName;
            to.LastName = from.LastName;
            to.Email = from.Email;
        }
        private async Task<User?> GetUserById(Guid id) => await DbSet.FirstOrDefaultAsync(u => u.Id == id);
    }
}