using Livros.Domain.Core.Interfaces.Repositories.Base;
using Livros.Domain.Entities;
using Livros.Domain.Pagination;

namespace Livros.Domain.Core.Interfaces.Repositories
{
    public interface IObraRepository : IRepositoryBase<Obra>
    {
        Task<PagedList<Obra>> GetPaginationAsync(ParametersObra parametersObra);

        Task<Obra> GetByIdDetalhesAsync(Guid obraId);

        bool ExisteId(Guid obraId);

        bool ExisteNome(Obra obra);

        bool ExisteVolumeId(Guid id);

        bool ExisteNumeroVolume(Volume volume);
    }
}