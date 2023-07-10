using Livros.Domain.Core.Interfaces.Repositories.Base;
using Livros.Domain.Entities;
using Livros.Domain.Pagination;

namespace Livros.Domain.Core.Interfaces.Repositories
{
    public interface IObraRepository : IRepositoryBase<Obra>
    {
        Task<PagedList<Obra>> GetPaginationAsync(ParametersObra parametersObra);

        bool ExisteId(Guid id);

        bool ExisteNome(Obra obra);
    }
}