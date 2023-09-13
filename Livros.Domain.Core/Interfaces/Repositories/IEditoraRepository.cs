using Livros.Domain.Core.Interfaces.Repositories.Base;
using Livros.Domain.Entities;
using Livros.Domain.Pagination;

namespace Livros.Domain.Core.Interfaces.Repositories
{
    public interface IEditoraRepository : IRepositoryBase<Editora>
    {
        Task<PagedList<Editora>> GetPaginationAsync(ParametersEditora parametersEditora);

        bool ExisteId(Guid id);

        bool ExisteNome(Editora editora);
    }
}