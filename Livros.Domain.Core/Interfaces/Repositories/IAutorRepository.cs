using Livros.Domain.Core.Interfaces.Repositories.Base;
using Livros.Domain.Entities;
using Livros.Domain.Pagination;

namespace Livros.Domain.Core.Interfaces.Repositories
{
    public interface IAutorRepository : IRepositoryBase<Autor>
    {
        Task<PagedList<Autor>> GetPaginationAsync(ParametersAutor parametersAutor);

        bool ExisteId(Guid id);

        bool ExisteNome(Autor autor);
    }
}