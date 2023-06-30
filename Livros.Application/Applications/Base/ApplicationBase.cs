using AutoMapper;
using Livros.Application.Dtos.Base;
using Livros.Application.Interfaces.Base;
using Livros.Application.Structs;
using Livros.Domain.Core.Interfaces.Services;
using Livros.Domain.Core.Interfaces.Services.Base;
using Livros.Domain.Core.Notificacoes;
using Livros.Domain.Core.Results;
using Livros.Domain.Entities.Base;
using Livros.Domain.Enums;

namespace Livros.Application.Applications.Base
{
    public class ApplicationBase<TEntity, TView, TPost, TPut, TStatus> :
         IApplicationBase<TEntity, TView, TPost, TPut, TStatus>
         where TEntity : EntityBase where TView : class where TPost : class where TPut : class where TStatus : PutStatusDto
    {
        protected readonly IMapper mapper;
        protected readonly IServiceBase<TEntity> serviceBase;
        protected readonly INotifier notifier;

        public ApplicationBase(IServiceBase<TEntity> serviceBase, INotifier notifier, IMapper mapper)
        {
            this.serviceBase = serviceBase;
            this.mapper = mapper;
            this.notifier = notifier;
        }

        protected void AddNotification(string messege)
        {
            notifier.AddNotification(new(messege));
        }

        protected void AddNotification(List<string> messege)
        {
            foreach (string text in messege)
            {
                notifier.AddNotification(new(text));
            }
        }

        protected void AddNotification(string messege, ENotificationType notificationType)
        {
            notifier.AddNotification(new(messege, notificationType));
        }

        protected void AddNotification(List<string> messege, ENotificationType notificationType)
        {
            foreach (string text in messege)
            {
                notifier.AddNotification(new(text, notificationType));
            }
        }

        public virtual async Task<IEnumerable<TView>> GetAllAsync()
        {
            IEnumerable<TEntity> entity = await serviceBase.GetAllAsync();
            return mapper.Map<IList<TView>>(entity);
        }

        public virtual async Task<TView> GetByIdAsync(Guid id, bool trackChanges)
        {
            ServiceResult<TEntity> entity = await serviceBase.GetByIdAsync(id, trackChanges);
            return mapper.Map<TView>(entity.Value);
        }

        public virtual async Task<EntityToDto<TEntity, TPut>> MapStructById(Guid id)
        {
            ServiceResult<TEntity> entity = await serviceBase.GetByIdAsync(id, false);
            return new EntityToDto<TEntity, TPut>(entity.Value, mapper.Map<TPut>(entity));
        }

        public virtual async Task<TView> PostAsync(TPost post)
        {
            TEntity entity = mapper.Map<TEntity>(post);
            entity = await serviceBase.PostAsync(entity);
            return mapper.Map<TView>(entity);
        }

        public virtual async Task<TView> PutAsync(TPut put)
        {
            TEntity entity = mapper.Map<TEntity>(put);
            entity = await serviceBase.PutAsync(entity);
            return mapper.Map<TView>(entity);
        }

        public virtual async Task<TView> DeleteAsync(Guid id)
        {
            TEntity entity = await serviceBase.DeleteAsync(id);
            return mapper.Map<TView>(entity);
        }

        public virtual async Task<TView> PutStatusAsync(Guid id, EStatus status)
        {
            TEntity entity = await serviceBase.PutStatusAsync(id, status);
            return mapper.Map<TView>(entity);
        }

        public virtual async Task<bool> ValidateRegister(Guid id)
        {
            return await serviceBase.ValidateRegister(id);
        }

        public virtual async Task MapStructSaveChangesAsync(EntityToDto<TEntity, TPut> dtoStruct)
        {
            TEntity entity = mapper.Map(dtoStruct.Dto, dtoStruct.Entity);
            await serviceBase.SaveChangesAsync(entity);
        }
    }
}