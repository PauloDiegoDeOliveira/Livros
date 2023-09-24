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
    public class GeneroRepository : RepositoryBase<Genero>, IGeneroRepository
    {
        private readonly IUser user;

        public GeneroRepository(AppDbContext appDbContext,
                                INotifier notifier,
                                ILogger<RepositoryBase<Genero>> logger,
                                IConfiguration configuration,
                                IUser user) : base(appDbContext, notifier, logger, configuration)
        {
            this.user = user;
        }

        public async Task<PagedList<Genero>> GetPaginationAsync(ParametersGenero parametersGenero)
        {
            return await TryCatch(async () =>
            {
                IQueryable<Genero> generos = appDbContext.Generos
                    .Where(g => g.UsuarioId == user.GetUserId().ToString())
                    .AsNoTracking();

                if (parametersGenero.Id != null)
                {
                    generos = generos.Where(g => parametersGenero.Id.Contains(g.Id));
                }

                if (parametersGenero.PalavraChave != null)
                {
                    generos = generos.Where(g => g.Nome.ToLower().Trim().Contains(parametersGenero.PalavraChave.ToLower().Trim()));
                }

                if (parametersGenero.Status != 0)
                {
                    generos = generos.Where(g => g.Status == parametersGenero.Status.ToString());
                }

                if (!generos.Any())
                {
                    AddNotification("Nenhum objeto foi encontrado.", ENotificationType.Warning);
                }
                else if (generos.Count() > 1)
                {
                    if (parametersGenero.Ordenar != 0)
                    {
                        switch (parametersGenero.Ordenar)
                        {
                            case EOrdenar.Crescente:
                                generos = generos.OrderBy(g => g.Nome);
                                break;

                            case EOrdenar.Decrescente:
                                generos = generos.OrderByDescending(g => g.Nome);
                                break;

                            case EOrdenar.Novos:
                                generos = generos.OrderByDescending(g => g.CriadoEm);
                                break;

                            case EOrdenar.Antigos:
                                generos = generos.OrderBy(g => g.CriadoEm);
                                break;
                        }
                    }
                    else
                    {
                        generos = generos.OrderBy(e => e.CriadoEm);
                    }
                }

                return await Task.FromResult(PagedList<Genero>.ToPagedList(generos, parametersGenero.NumeroPagina, parametersGenero.ResultadosExibidos));
            });
        }

        private void DefinirUsuarioId(Genero genero)
        {
            genero.UsuarioId = user.GetUserId().ToString();
        }

        public override async Task<Genero> PostAsync(Genero genero)
        {
            DefinirUsuarioId(genero);
            return await base.PostAsync(genero);
        }

        public override async Task<Genero> PutAsync(Genero genero)
        {
            DefinirUsuarioId(genero);
            return await base.PutAsync(genero);
        }

        public bool ExisteId(Guid id)
        {
            return appDbContext.Generos.Any(g => g.Id == id);
        }

        public bool ExisteNome(Genero genero)
        {
            IQueryable<Genero> query = appDbContext.Generos.AsNoTracking()
                .Where(g => g.Nome.ToLower() == genero.Nome.ToLower());

            if (genero.Id != Guid.Empty)
            {
                query = query.Where(g => g.Id != genero.Id);
            }

            return query.Any();
        }
    }
}