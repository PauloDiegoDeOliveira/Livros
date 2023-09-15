using Livros.Domain.Core.Interfaces.Services.Base;
using Livros.Domain.Entities;
using Livros.Domain.Pagination;

namespace Livros.Domain.Core.Interfaces.Services
{
    public interface IIdiomaService : IServiceBase<Idioma>
    {
        Task<PagedList<Idioma>> GetPaginationAsync(ParametersIdioma parametersIdioma);

        bool ExisteId(Guid id);

        bool ExisteNome(Idioma idioma);
    }
}