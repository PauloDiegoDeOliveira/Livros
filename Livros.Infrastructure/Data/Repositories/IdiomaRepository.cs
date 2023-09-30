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
    public class IdiomaRepository : RepositoryBase<Idioma>, IIdiomaRepository
    {
        public IdiomaRepository(AppDbContext appDbContext,
                                INotifier notifier,
                                ILogger<RepositoryBase<Idioma>> logger,
                                IConfiguration configuration,
                                IUser user) : base(appDbContext, notifier, logger, configuration)
        {
        }

        public async Task<PagedList<Idioma>> GetPaginationAsync(ParametersIdioma parametersIdioma)
        {
            return await TryCatch(async () =>
            {
                IQueryable<Idioma> idiomas = appDbContext.Idiomas.AsNoTracking();

                if (parametersIdioma.Id != null)
                {
                    idiomas = idiomas.Where(g => parametersIdioma.Id.Contains(g.Id));
                }

                if (parametersIdioma.PalavraChave != null)
                {
                    idiomas = idiomas.Where(g => g.Nome.ToLower().Trim().Contains(parametersIdioma.PalavraChave.ToLower().Trim()));
                }

                if (parametersIdioma.Status != 0)
                {
                    idiomas = idiomas.Where(g => g.Status == parametersIdioma.Status.ToString());
                }

                if (!idiomas.Any())
                {
                    AddNotification("Nenhum objeto foi encontrado.", ENotificationType.Warning);
                }
                else if (idiomas.Count() > 1)
                {
                    if (parametersIdioma.Ordenar != 0)
                    {
                        switch (parametersIdioma.Ordenar)
                        {
                            case EOrdenar.Crescente:
                                idiomas = idiomas.OrderBy(g => g.Nome);
                                break;

                            case EOrdenar.Decrescente:
                                idiomas = idiomas.OrderByDescending(g => g.Nome);
                                break;

                            case EOrdenar.Novos:
                                idiomas = idiomas.OrderByDescending(g => g.CriadoEm);
                                break;

                            case EOrdenar.Antigos:
                                idiomas = idiomas.OrderBy(g => g.CriadoEm);
                                break;
                        }
                    }
                    else
                    {
                        idiomas = idiomas.OrderBy(e => e.CriadoEm);
                    }
                }

                return await Task.FromResult(PagedList<Idioma>.ToPagedList(idiomas, parametersIdioma.NumeroPagina, parametersIdioma.ResultadosExibidos));
            });
        }

        public bool ExisteId(Guid id)
        {
            return appDbContext.Idiomas.Any(g => g.Id == id);
        }

        public bool ExisteNome(Idioma idioma)
        {
            IQueryable<Idioma> query = appDbContext.Idiomas.AsNoTracking()
                .Where(i => i.Nome.ToLower() == idioma.Nome.ToLower());

            if (idioma.Id != Guid.Empty)
            {
                query = query.Where(i => i.Id != idioma.Id);
            }

            return query.Any();
        }
    }
}