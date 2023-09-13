using Livros.Domain.Core.Interfaces.Services.Base;
using Livros.Domain.Entities;
using Livros.Domain.Pagination;

namespace Livros.Domain.Core.Interfaces.Services
{
    public interface IEditoraService : IServiceBase<Editora>
    {
        Task<PagedList<Editora>> GetPaginationAsync(ParametersEditora parametersEditora);

        bool ExisteId(Guid id);

        bool ExisteNome(Editora editora);
    }
}