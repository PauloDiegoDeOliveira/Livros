using Livros.Domain.Core.Interfaces.Repositories;
using Livros.Domain.Core.Interfaces.Services;
using Livros.Domain.Core.Notificacoes;
using Livros.Domain.Entities;
using Livros.Domain.Enums;
using Livros.Domain.Pagination;
using Livros.Infrastructure.Data.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Livros.Infrastructure.Data.Repositories
{
    public class ObraRepository : RepositoryBase<Obra>, IObraRepository
    {
        private readonly AppDbContext appDbContext;
        private readonly IUser user;

        public ObraRepository(AppDbContext appDbContext,
                              INotifier notifier,
                              ILogger<RepositoryBase<Obra>> logger,
                              IConfiguration configuration,
                              IUser user) : base(appDbContext, notifier, logger, configuration)
        {
            this.appDbContext = appDbContext;
            this.user = user;
        }

        public async Task<PagedList<Obra>> GetPaginationAsync(ParametersObra parametersObra)
        {
            return await TryCatch(async () =>
            {
                IQueryable<Obra> obras = appDbContext.Obras
                        .Include(o => o.Volumes).AsNoTracking();

                if (parametersObra.Id != null)
                {
                    obras = obras.Where(o => parametersObra.Id.Contains(o.Id));
                }

                if (parametersObra.PalavraChave != null)
                {
                    obras = obras.Where(o => o.Titulo.ToLower().Trim().Contains(parametersObra.PalavraChave.ToLower().Trim()));
                }

                if (parametersObra.Status != 0)
                {
                    obras = obras.Where(o => o.Status == parametersObra.Status.ToString());
                }

                if (!obras.Any())
                {
                    AddNotification("Nenhum objeto foi encontrado.", ENotificationType.Warning);
                }
                else if (obras.Count() > 1)
                {
                    if (parametersObra.Ordenar != 0)
                    {
                        switch (parametersObra.Ordenar)
                        {
                            case EOrdenar.Crescente:
                                obras = obras.OrderBy(o => o.Titulo);
                                break;

                            case EOrdenar.Decrescente:
                                obras = obras.OrderByDescending(o => o.Titulo);
                                break;

                            case EOrdenar.Novos:
                                obras = obras.OrderByDescending(o => o.CriadoEm);
                                break;

                            case EOrdenar.Antigos:
                                obras = obras.OrderBy(o => o.CriadoEm);
                                break;
                        }
                    }
                    else
                    {
                        obras = obras.OrderBy(o => o.CriadoEm);
                    }
                }

                return await Task.FromResult(PagedList<Obra>.ToPagedList(obras, parametersObra.NumeroPagina, parametersObra.ResultadosExibidos));
            });
        }

        public override async Task<Obra> PostAsync(Obra obra)
        {
            obra.UsuarioId = user.GetUserId().ToString();
            return await base.PostAsync(obra);
        }

        public override async Task<Obra> PutAsync(Obra obra)
        {
            obra.UsuarioId = user.GetUserId().ToString();
            return await base.PutAsync(obra);
        }

        public bool ExisteId(Guid id)
        {
            return appDbContext.Obras.Any(o => o.Id == id);
        }

        public bool ExisteNome(Obra obra)
        {
            if (obra.Id == Guid.Empty)
            {
                return appDbContext.Obras.AsNoTracking().Any(o => o.Titulo.ToLower() == obra.Titulo.ToLower());
            }
            else
            {
                return appDbContext.Obras.AsNoTracking()
                           .Any(o => o.Titulo.ToLower() == obra.Titulo.ToLower()
                               && o.Id != obra.Id);
            }
        }
    }
}