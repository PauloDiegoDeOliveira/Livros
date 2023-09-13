using Livros.Domain.Core.Interfaces.Services.Base;
using Livros.Domain.Entities;
using Livros.Domain.Pagination;

namespace Livros.Domain.Core.Interfaces.Services
{
    public interface IGeneroService : IServiceBase<Genero>
    {
        Task<PagedList<Genero>> GetPaginationAsync(ParametersGenero parametersGenero);

        bool ExisteId(Guid id);

        bool ExisteNome(Genero genero);
    }
}