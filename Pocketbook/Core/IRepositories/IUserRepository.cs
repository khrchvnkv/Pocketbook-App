using Pocketbook.Models;

namespace Pocketbook.Core.IRepositories
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<string?> GetFirstNameAndLastName(Guid id);
    }
}