using Livros.Domain.Core.Interfaces.Repositories.Base;
using Livros.Domain.Entities;
using Livros.Domain.Pagination;

namespace Livros.Domain.Core.Interfaces.Repositories
{
    public interface IGeneroRepository : IRepositoryBase<Genero>
    {
        Task<PagedList<Genero>> GetPaginationAsync(ParametersGenero parametersGenero);

        bool ExisteId(Guid id);

        bool ExisteNome(Genero genero);
    }
}