using Livros.Application.Dtos.Base;
using Livros.Application.Structs;
using Livros.Domain.Entities.Base;
using Livros.Domain.Enums;

namespace Livros.Application.Interfaces.Base
{
    public interface IApplicationBase<TEntity, TView, TPost, TPut, TStatus>
            where TView : class where TPost : class where TPut : class where TEntity : EntityBase where TStatus : PutStatusDto
    {
        Task<IEnumerable<TView>> GetAllAsync();

        Task<TView> GetByIdAsync(Guid id, bool trackChanges);

        Task<EntityToDto<TEntity, TPut>> MapStructById(Guid id);

        Task<TView> PostAsync(TPost post);

        Task<TView> PutAsync(TPut put);

        Task<TView> PutStatusAsync(Guid id, EStatus status);

        Task<TView> DeleteAsync(Guid id);

        Task<bool> ValidateRegister(Guid id);

        Task MapStructSaveChangesAsync(EntityToDto<TEntity, TPut> dtoStruct);
    }
}