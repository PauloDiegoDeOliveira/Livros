using Livros.Domain.Core.Interfaces.Services.Base;
using Livros.Domain.Entities;
using Livros.Domain.Pagination;

namespace Livros.Domain.Core.Interfaces.Services
{
    public interface IObraService : IServiceBase<Obra>
    {
        Task<PagedList<Obra>> GetPaginationAsync(ParametersObra parametersObra);

        Task<Obra> GetByIdDetalhesAsync(Guid obraId);

        bool ExisteId(Guid obraId);

        bool ExisteNome(Obra obra);
    }
}