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
    public class EstanteRepository : RepositoryBase<Estante>, IEstanteRepository
    {
        private readonly IUser user;

        public EstanteRepository(AppDbContext appDbContext,
                                 INotifier notifier,
                                 ILogger<RepositoryBase<Estante>> logger,
                                 IConfiguration configuration,
                                 IUser user) : base(appDbContext, notifier, logger, configuration)
        {
            this.user = user;
        }

        public async Task<PagedList<Estante>> GetPaginationAsync(ParametersEstante parametersEstante)
        {
            return await TryCatch(async () =>
            {
                IQueryable<Estante> estantes = appDbContext.Estantes
                                .Where(e => e.UsuarioId == user.GetUserId().ToString())
                                .AsNoTracking();

                if (parametersEstante.Id != null)
                {
                    estantes = estantes.Where(e => parametersEstante.Id.Contains(e.Id));
                }

                if (parametersEstante.PalavraChave != null)
                {
                    estantes = estantes.Where(e => e.Nome.ToLower().Trim().Contains(parametersEstante.PalavraChave.ToLower().Trim()));
                }

                if (parametersEstante.Status != 0)
                {
                    estantes = estantes.Where(e => e.Status == parametersEstante.Status.ToString());
                }

                if (!estantes.Any())
                {
                    AddNotification("Nenhum objeto foi encontrado.", ENotificationType.Warning);
                }
                else if (estantes.Count() > 1)
                {
                    if (parametersEstante.Ordenar != 0)
                    {
                        switch (parametersEstante.Ordenar)
                        {
                            case EOrdenar.Crescente:
                                estantes = estantes.OrderBy(e => e.Nome);
                                break;

                            case EOrdenar.Decrescente:
                                estantes = estantes.OrderByDescending(e => e.Nome);
                                break;

                            case EOrdenar.Novos:
                                estantes = estantes.OrderByDescending(e => e.CriadoEm);
                                break;

                            case EOrdenar.Antigos:
                                estantes = estantes.OrderBy(e => e.CriadoEm);
                                break;
                        }
                    }
                    else
                    {
                        estantes = estantes.OrderBy(e => e.CriadoEm);
                    }
                }

                return await Task.FromResult(PagedList<Estante>.ToPagedList(estantes, parametersEstante.NumeroPagina, parametersEstante.ResultadosExibidos));
            });
        }

        public override async Task<Estante> PostAsync(Estante estante)
        {
            estante.UsuarioId = user.GetUserId().ToString();
            return await base.PostAsync(estante);
        }

        public override async Task<Estante> PutAsync(Estante estante)
        {
            estante.UsuarioId = user.GetUserId().ToString();
            return await base.PutAsync(estante);
        }

        public bool ExisteId(Guid id)
        {
            return appDbContext.Estantes.Any(e => e.Id == id);
        }

        public bool ExisteNome(Estante estante)
        {
            if (estante.Id == Guid.Empty)
            {
                return appDbContext.Estantes.AsNoTracking()
                             .Any(e => e.Nome.ToLower() == estante.Nome.ToLower());
            }
            else
            {
                return appDbContext.Estantes.AsNoTracking()
                            .Any(e => e.Nome.ToLower() == estante.Nome.ToLower()
                                && e.Id != estante.Id);
            }
        }
    }
}