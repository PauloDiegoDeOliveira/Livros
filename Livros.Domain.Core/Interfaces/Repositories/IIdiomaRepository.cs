using Livros.Domain.Core.Interfaces.Repositories.Base;
using Livros.Domain.Entities;
using Livros.Domain.Pagination;

namespace Livros.Domain.Core.Interfaces.Repositories
{
    public interface IIdiomaRepository : IRepositoryBase<Idioma>
    {
        Task<PagedList<Idioma>> GetPaginationAsync(ParametersIdioma parametersIdioma);

        bool ExisteId(Guid id);

        bool ExisteNome(Idioma idioma);
    }
}