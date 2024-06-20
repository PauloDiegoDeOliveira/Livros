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
                IQueryable<Autor> autores = appDbContext.Autores.Where(a => a.UsuarioId == user.GetUserId().ToString())
                    .Include(a => a.Obras.Where(o => o.UsuarioId == user.GetUserId().ToString()))
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

                if (parametersAutor.QuantidadeObras != 0)
                {
                    switch (parametersAutor.QuantidadeObras)
                    {
                        case EQuantidadeObras.Crescente:
                            autores = autores.OrderBy(e => e.Obras.Count());
                            break;

                        case EQuantidadeObras.Decrescente:
                            autores = autores.OrderByDescending(e => e.Obras.Count());
                            break;
                    }
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
                }

                return await Task.FromResult(PagedList<Autor>.ToPagedList(autores, parametersAutor.NumeroPagina, parametersAutor.ResultadosExibidos));
            });
        }

        private void DefinirUsuarioId(Autor autor)
        {
            autor.UsuarioId = user.GetUserId().ToString();
        }

        public override async Task<Autor> PostAsync(Autor autor)
        {
            DefinirUsuarioId(autor);
            return await base.PostAsync(autor);
        }

        public override async Task<Autor> PutAsync(Autor autor)
        {
            DefinirUsuarioId(autor);
            return await base.PutAsync(autor);
        }

        public bool ExisteId(Guid id)
        {
            return appDbContext.Autores.Any(g => g.Id == id);
        }

        public bool ExisteNome(Autor autor)
        {
            IQueryable<Autor> query = appDbContext.Autores.AsNoTracking()
                .Where(o => o.Nome.ToLower() == autor.Nome.ToLower()
                       && o.UsuarioId == user.GetUserId().ToString()
                       && o.Status != EStatus.Excluido.ToString());

            if (autor.Id != Guid.Empty)
            {
                query = query.Where(a => a.Id != autor.Id);
            }

            return query.Any();
        }
    }
}