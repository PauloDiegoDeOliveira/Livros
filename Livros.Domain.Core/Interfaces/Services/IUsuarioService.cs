using Livros.Domain.Core.Results;
using Livros.Domain.Entities;
using Livros.Domain.Enums;
using Livros.Domain.Pagination;

namespace Livros.Domain.Core.Interfaces.Services
{
    public interface IUsuarioService
    {
        Task<PagedList<Usuario>> GetPaginationAsync(ParametersUsuario parametersUsuario);

        Task<ServiceResult<Usuario>> GetByIdAsync(Guid id, bool trackChanges);

        Task<List<Usuario>> GetListUsuariosByIds(List<Guid> Ids);

        Task<Usuario> AlteraUltimoAcesso(Guid id);

        Task<Usuario> PutStatusAsync(Guid id, EStatus status);

        bool ValidarId(Guid id);

        Task<bool> VersaoToken(Guid usuarioId);
    }
}