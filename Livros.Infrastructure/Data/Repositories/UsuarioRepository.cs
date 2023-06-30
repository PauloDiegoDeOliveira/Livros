using Livros.Domain.Core.Interfaces.Repositories;
using Livros.Domain.Core.Interfaces.Services;
using Livros.Domain.Core.Notificacoes;
using Livros.Domain.Core.Results;
using Livros.Domain.Entities;
using Livros.Domain.Enums;
using Livros.Domain.Pagination;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Livros.Infrastructure.Data.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly AppDbContext appDbContext;
        private readonly INotifier notifier;
        private readonly ILogger<UsuarioRepository> logger;
        private readonly IUser user;

        public UsuarioRepository(AppDbContext appDbContext,
                                 INotifier notifier,
                                 ILogger<UsuarioRepository> logger,
                                 IUser user)
        {
            this.appDbContext = appDbContext;
            this.notifier = notifier;
            this.logger = logger;
            this.user = user;
        }

        public async Task<PagedList<Usuario>> GetPaginationAsync(ParametersUsuario parametersUsuario)
        {
            return await TryCatch(async () =>
            {
                IQueryable<Usuario> usuarios = appDbContext.Users.AsNoTracking();

                if (parametersUsuario.Id != null)
                {
                    string[] guids = parametersUsuario.Id.Select(x => x.ToString()).ToArray();
                    usuarios = usuarios.Where(usuario => guids.Contains(usuario.Id));
                }

                if (!string.IsNullOrEmpty(parametersUsuario.PalavraChave))
                {
                    usuarios = usuarios.Where(usuario => usuario.Nome.ToLower().Trim().Contains(parametersUsuario.PalavraChave.ToLower().Trim()));
                }

                if (parametersUsuario.Status != 0)
                {
                    usuarios = usuarios.Where(usuario => usuario.Status == parametersUsuario.Status.ToString());
                }
                else
                {
                    usuarios = usuarios.Where(usuario => usuario.Status != EStatus.Excluido.ToString());
                }

                if (!usuarios.Any())
                {
                    AddNotification("Nenhum objeto foi encontrado.", ENotificationType.Warning);
                }

                return await Task.FromResult(PagedList<Usuario>.ToPagedList(usuarios, parametersUsuario.NumeroPagina, parametersUsuario.ResultadosExibidos));
            });
        }

        public async Task<List<Usuario>> GetListUsuariosByIds(List<Guid> Ids)
        {
            return await TryCatch(async () =>
            {
                string[] guids = Ids.Select(x => x.ToString()).ToArray();
                List<Usuario> usuario = await appDbContext.Users.AsNoTracking().Where(x => guids.Contains(x.Id)).ToListAsync();
                if (!usuario.Any())
                {
                    AddNotification("Nenhum objeto foi encontrado.", ENotificationType.Warning);
                }

                return usuario;
            });
        }

        public async Task<ServiceResult<Usuario>> GetByIdAsync(Guid id, bool trackChanges = true)
        {
            return await TryCatch(async () =>
            {
                Usuario entity;
                if (!trackChanges)
                {
                    entity = await appDbContext.Users.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id.ToString());
                }
                else
                {
                    entity = await appDbContext.Users.SingleOrDefaultAsync(x => x.Id == id.ToString());
                }

                if (entity is null)
                {
                    AddNotification("Nenhum objeto foi encontrado com o id informado.");
                    return ServiceResult<Usuario>.Error();
                }

                return ServiceResult<Usuario>.Success(entity);
            });
        }

        public async Task<Usuario> AlteraUltimoAcesso(Guid id)
        {
            return await TryCatch(async () =>
            {
                ServiceResult<Usuario> entityResult = await GetByIdAsync(id, true);
                if (entityResult is null || !entityResult.IsSuccess)
                {
                    return null;
                }

                entityResult.Value.UltimoAcesso = DateTimeOffset.Now;

                await SaveChangesAsync();

                return entityResult.Value;
            });
        }

        public virtual async Task<Usuario> PutStatusAsync(Guid id, EStatus status)
        {
            ServiceResult<Usuario> entityResult = await GetByIdAsync(id, true);
            if (entityResult is null || !entityResult.IsSuccess)
            {
                return null;
            }

            entityResult.Value.Status = status.ToString();

            appDbContext.Entry(entityResult.Value).State = EntityState.Modified;

            await SaveChangesAsync();

            return entityResult.Value;
        }

        public async Task<bool> VersaoToken(Guid usuarioId)
        {
            Usuario usuario = await appDbContext.Users.FirstOrDefaultAsync(usuario => usuario.Id == usuarioId.ToString());
            if (usuario == null)
            {
                return false;
            }

            usuario.VersaoToken++;

            await appDbContext.SaveChangesAsync();

            return true;
        }

        public bool ValidarId(Guid id)
        {
            return appDbContext.Users.Any(x => x.Id == id.ToString());
        }

        public async Task<int> SaveChangesAsync()
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
                logger.LogWarning(ex.ToString());
                //logger.LogWarning(ex.Message);
                //logger.LogWarning(ex.StackTrace);

                return default;
            }
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
    }
}