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
    public class AutorRepository : RepositoryBase<Autor>, IAutorRepository
    {
        private readonly IUser user;

        public AutorRepository(AppDbContext appDbContext,
                                INotifier notifier,
                                ILogger<RepositoryBase<Autor>> logger,
                                IConfiguration configuration,
                                IUser user) : base(appDbContext, notifier, logger, configuration)
        {
            this.user = user;
        }

        public async Task<PagedList<Autor>> GetPaginationAsync(ParametersAutor parametersAutor)
        {
            return await TryCatch(async () =>
            {
                IQueryable<Autor> autores = appDbContext.Autores
                    .Where(g => g.UsuarioId == user.GetUserId().ToString())
                    .AsNoTracking();

                if (parametersAutor.Id != null)
                {
                    autores = autores.Where(g => parametersAutor.Id.Contains(g.Id));
                }

                if (parametersAutor.PalavraChave != null)
                {
                    autores = autores.Where(g => g.Nome.ToLower().Trim().Contains(parametersAutor.PalavraChave.ToLower().Trim()));
                }

                if (parametersAutor.Status != 0)
                {
                    autores = autores.Where(g => g.Status == parametersAutor.Status.ToString());
                }

                if (!autores.Any())
                {
                    AddNotification("Nenhum objeto foi encontrado.", ENotificationType.Warning);
                }
                else if (autores.Count() > 1)
                {
                    if (parametersAutor.Ordenar != 0)
                    {
                        switch (parametersAutor.Ordenar)
                        {
                            case EOrdenar.Crescente:
                                autores = autores.OrderBy(g => g.Nome);
                                break;

                            case EOrdenar.Decrescente:
                                autores = autores.OrderByDescending(g => g.Nome);
                                break;

                            case EOrdenar.Novos:
                                autores = autores.OrderByDescending(g => g.CriadoEm);
                                break;

                            case EOrdenar.Antigos:
                                autores = autores.OrderBy(g => g.CriadoEm);
                                break;
                        }
                    }
                    else
                    {
                        autores = autores.OrderBy(e => e.CriadoEm);
                    }
                }

                return await Task.FromResult(PagedList<Autor>.ToPagedList(autores, parametersAutor.NumeroPagina, parametersAutor.ResultadosExibidos));
            });
        }

        public override async Task<Autor> PostAsync(Autor autor)
        {
            autor.UsuarioId = user.GetUserId().ToString();
            return await base.PostAsync(autor);
        }

        public override async Task<Autor> PutAsync(Autor autor)
        {
            autor.UsuarioId = user.GetUserId().ToString();
            return await base.PutAsync(autor);
        }

        public bool ExisteId(Guid id)
        {
            return appDbContext.Autores.Any(g => g.Id == id);
        }

        public bool ExisteNome(Autor autor)
        {
            if (autor.Id == Guid.Empty)
            {
                return appDbContext.Autores.AsNoTracking()
                       .Any(g => g.Nome.ToLower() == autor.Nome.ToLower());
            }
            else
            {
                return appDbContext.Autores.AsNoTracking()
                        .Any(g => g.Nome.ToLower() == autor.Nome.ToLower()
                            && g.Id != autor.Id);
            }
        }
    }
}