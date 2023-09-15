using Livros.Domain.Core.Interfaces.Services.Base;
using Livros.Domain.Entities;
using Livros.Domain.Pagination;

namespace Livros.Domain.Core.Interfaces.Services
{
    public interface IEstanteService : IServiceBase<Estante>
    {
        Task<PagedList<Estante>> GetPaginationAsync(ParametersEstante parametersEstante);

        bool ExisteId(Guid id);

        bool ExisteNome(Estante estante);
    }
}