using Pocketbook.Core.IRepositories;

namespace Pocketbook.Core.IConfiguration
{
    public interface IUnitOfWork
    {
        IUserRepository Users { get; }

        Task CompleteAsync();
    }
}