using Microsoft.EntityFrameworkCore;
using Pocketbook.Core.IRepositories;
using Pocketbook.Data;
using Pocketbook.Models;

namespace Pocketbook.Core.Repositories
{
    public abstract class GenericRepository<T>: IGenericRepository<T> where T : class, IEntity
    {
        protected readonly ApplicationDbContext Context;
        protected readonly DbSet<T> DbSet;
        protected readonly ILogger Logger;

        protected GenericRepository(ApplicationDbContext context, ILogger logger)
        {
            Context = context;
            DbSet = context.Set<T>();
            Logger = logger;
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            try
            {
                return await DbSet.ToListAsync();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "{Repo} GetAll method error", GetType().Name);
                return new List<T>();
            }
        }
        public virtual async Task<T?> GetById(Guid id)
        {
            try
            {
                return await DbSet.FindAsync(id);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "{Repo} GetById method error", GetType().Name);
                return null;
            }
        }
        public virtual async Task<bool> Add(T entity)
        {
            try
            {
                await DbSet.AddAsync(entity);
                return true;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "{Repo} Add method error", GetType().Name);
                return false;
            }
        }
        public virtual async Task<bool> Delete(Guid id)
        {
            try
            {
                var entity = await GetById(id);

                if (entity is not null)
                {
                    DbSet.Remove(entity);
                    return true;
                }
                
                return false;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "{Repo} Delete method error", GetType().Name);
                return false;
            }
        }
        public virtual async Task<bool> Update(T entity)
        {
            try
            {
                var dbEntity = await GetById(entity.Id);
                if (dbEntity is null) return await Add(entity);

                CopyDataOnUpdating(entity, dbEntity);
                return true;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "{Repo} Update method error", GetType().Name);
                return false;
            }
        }
        protected abstract void CopyDataOnUpdating(T from, T to);
    }
}