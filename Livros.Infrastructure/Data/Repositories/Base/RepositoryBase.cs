using Livros.Domain.Core.Interfaces.Repositories.Base;
using Livros.Domain.Core.Interfaces.Services;
using Livros.Domain.Core.Notificacoes;
using Livros.Domain.Core.Results;
using Livros.Domain.Entities.Base;
using Livros.Domain.Enums;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Livros.Infrastructure.Data.Repositories.Base
{
    public class RepositoryBase<TEntity> : IRepositoryBase<TEntity> where TEntity : EntityBase
    {
        private readonly AppDbContext appDbContext;
        private readonly INotifier notifier;
        private readonly ILogger<RepositoryBase<TEntity>> logger;
        protected SqlConnection sqlConnection;
        protected IConfiguration configuration;

        public RepositoryBase(AppDbContext appDbContext,
                              INotifier notifier,
                              ILogger<RepositoryBase<TEntity>> logger,
                              IConfiguration configuration)
        {
            this.appDbContext = appDbContext;
            this.notifier = notifier;
            this.logger = logger;
            this.configuration = configuration;
            this.sqlConnection = new SqlConnection(configuration.GetConnectionString("Connection"));
        }

        protected void AddNotification(string mensagem)
        {
            notifier.AddNotification(new Notification(mensagem));
        }

        protected void AddNotification(List<string> messege)
        {
            foreach (string text in messege)
            {
                notifier.AddNotification(new(text));
            }
        }

        protected void AddNotification(string mensagem, ENotificationType notificationType)
        {
            notifier.AddNotification(new Notification(mensagem, notificationType));
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
            return await TryCatch(async () =>
            {
                return await appDbContext.Set<TEntity>().AsNoTracking().ToListAsync();
            });
        }

        public async Task<ServiceResult<TEntity>> GetByIdAsync(Guid id, bool trackChanges = true)
        {
            return await TryCatch(async () =>
            {
                TEntity entity;
                if (!trackChanges)
                {
                    entity = await appDbContext.Set<TEntity>().AsNoTracking().SingleOrDefaultAsync(x => x.Id == id);
                }
                else
                {
                    entity = await appDbContext.Set<TEntity>().SingleOrDefaultAsync(x => x.Id == id);
                }

                if (entity is null)
                {
                    AddNotification("Nenhum objeto foi encontrado com o id informado.");

                    return ServiceResult<TEntity>.Error();
                }

                return ServiceResult<TEntity>.Success(entity);
            });
        }

        public virtual async Task<TEntity> PostAsync(TEntity entity)
        {
            await appDbContext.Set<TEntity>().AddAsync(entity);
            await SaveChangesAsync();

            return entity;
        }

        public virtual async Task<TEntity> PutAsync(TEntity entity)
        {
            ServiceResult<TEntity> entityResult = await GetByIdAsync(entity.Id, false);
            if (entityResult is null || !entityResult.IsSuccess)
            {
                return null;
            }

            appDbContext.Entry(entity).State = EntityState.Modified;
            await SaveChangesAsync();

            return entity;
        }

        public virtual async Task<TEntity> PutStatusAsync(Guid id, EStatus status)
        {
            ServiceResult<TEntity> entityResult = await GetByIdAsync(id, true);
            if (entityResult is null || !entityResult.IsSuccess)
            {
                return null;
            }

            entityResult.Value.Status = status.ToString();

            appDbContext.Entry(entityResult.Value).State = EntityState.Modified;
            await SaveChangesAsync();

            return entityResult.Value;
        }

        public virtual async Task<TEntity> DeleteAsync(Guid id)
        {
            ServiceResult<TEntity> entityResult = await GetByIdAsync(id, true);
            if (entityResult is null || !entityResult.IsSuccess)
            {
                return null;
            }

            appDbContext.Remove(entityResult.Value);
            await SaveChangesAsync();

            return entityResult.Value;
        }

        public virtual async Task<bool> ValidateRegister(Guid id)
        {
            return await appDbContext.Set<TEntity>().AnyAsync(x => x.Id == id);
        }

        public virtual async Task ModifyObject(TEntity entity)
        {
            appDbContext.Entry(entity).State = EntityState.Modified;
            await SaveChangesAsync();
        }

        public virtual async Task<int> SaveChangesAsync()
        {
            return await TryCatch(async () =>
            {
                int value = await appDbContext.SaveChangesAsync();
                if (value == 0)
                {
                    AddNotification("Nenhum dado foi alterado no banco.");
                }

                return value;
            });
        }

        public async Task<T> TryCatch<T>(Func<Task<T>> func)
        {
            try
            {
                return await func.Invoke();
            }
            catch (Exception ex)
            {
                AddNotification("Ocorreu um erro ao executar a operação.");
                logger.LogWarning(ex.InnerException.ToString());
                logger.LogWarning(ex.Message);
                //logger.LogWarning(ex.StackTrace);
                //logger.LogWarning(ex.ToString());

                return default;
            }
        }
    }
}