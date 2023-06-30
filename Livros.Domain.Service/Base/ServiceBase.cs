using Livros.Domain.Core.Interfaces.Repositories.Base;
using Livros.Domain.Core.Interfaces.Services;
using Livros.Domain.Core.Interfaces.Services.Base;
using Livros.Domain.Core.Notificacoes;
using Livros.Domain.Core.Results;
using Livros.Domain.Entities.Base;
using Livros.Domain.Enums;

namespace Livros.Domain.Service.Base
{
    public abstract class ServiceBase<TEntity> : IServiceBase<TEntity> where TEntity : EntityBase
    {
        private readonly IRepositoryBase<TEntity> repositoryBase;
        private readonly INotifier notifier;

        public ServiceBase(IRepositoryBase<TEntity> repositoryBase,
                           INotifier notifier)
        {
            this.repositoryBase = repositoryBase;
            this.notifier = notifier;
        }

        protected void AddNotification(string messege)
        {
            notifier.AddNotification(new Notification(messege));
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

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await repositoryBase.GetAllAsync();
        }

        public virtual async Task<ServiceResult<TEntity>> GetByIdAsync(Guid id, bool trackChanges)
        {
            return await repositoryBase.GetByIdAsync(id, trackChanges);
        }

        public virtual async Task<TEntity> PostAsync(TEntity entity)
        {
            return await repositoryBase.PostAsync(entity);
        }

        public virtual async Task<TEntity> PutAsync(TEntity entity)
        {
            return await repositoryBase.PutAsync(entity);
        }

        public virtual async Task<TEntity> PutStatusAsync(Guid id, EStatus status)
        {
            return await repositoryBase.PutStatusAsync(id, status);
        }

        public virtual async Task<TEntity> DeleteAsync(Guid id)
        {
            return await repositoryBase.DeleteAsync(id);
        }

        public virtual async Task<bool> ValidateRegister(Guid id)
        {
            return await repositoryBase.ValidateRegister(id);
        }

        public virtual async Task SaveChangesAsync(TEntity entity)
        {
            await repositoryBase.ModifyObject(entity);
        }
    }
}