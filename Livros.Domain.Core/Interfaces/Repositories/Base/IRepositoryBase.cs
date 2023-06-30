using Livros.Domain.Core.Results;
using Livros.Domain.Entities.Base;
using Livros.Domain.Enums;

namespace Livros.Domain.Core.Interfaces.Repositories.Base
{
    public interface IRepositoryBase<TEntity> where TEntity : EntityBase
    {
        Task<IEnumerable<TEntity>> GetAllAsync();

        Task<ServiceResult<TEntity>> GetByIdAsync(Guid id, bool trackChanges);

        Task<TEntity> PostAsync(TEntity entity);

        Task<TEntity> PutAsync(TEntity entity);

        Task<TEntity> PutStatusAsync(Guid id, EStatus status);

        Task<TEntity> DeleteAsync(Guid id);

        Task<bool> ValidateRegister(Guid id);

        Task ModifyObject(TEntity entity);
    }
}