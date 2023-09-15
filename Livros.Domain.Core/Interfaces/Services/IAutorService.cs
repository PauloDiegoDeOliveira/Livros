using Livros.Domain.Core.Interfaces.Services.Base;
using Livros.Domain.Entities;
using Livros.Domain.Pagination;

namespace Livros.Domain.Core.Interfaces.Services
{
    public interface IAutorService : IServiceBase<Autor>
    {
        Task<PagedList<Autor>> GetPaginationAsync(ParametersAutor parametersAutor);

        bool ExisteId(Guid id);

        bool ExisteNome(Autor autor);
    }
}