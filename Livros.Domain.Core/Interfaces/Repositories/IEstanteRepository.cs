using Livros.Domain.Core.Interfaces.Repositories.Base;
using Livros.Domain.Entities;
using Livros.Domain.Pagination;

namespace Livros.Domain.Core.Interfaces.Repositories
{
    public interface IEstanteRepository : IRepositoryBase<Estante>
    {
        Task<PagedList<Estante>> GetPaginationAsync(ParametersEstante parametersEstante);

        Task<Estante> GetByIdDetalhesAsync(Guid id);

        bool ExisteId(Guid id);

        bool ExisteNome(Estante estante);
    }
}