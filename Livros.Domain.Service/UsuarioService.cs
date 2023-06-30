using Livros.Domain.Core.Interfaces.Repositories;
using Livros.Domain.Core.Interfaces.Services;
using Livros.Domain.Core.Results;
using Livros.Domain.Entities;
using Livros.Domain.Enums;
using Livros.Domain.Pagination;

namespace Livros.Domain.Service
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository usuarioRepository;

        public UsuarioService(IUsuarioRepository usuarioRepository)
        {
            this.usuarioRepository = usuarioRepository;
        }

        public async Task<PagedList<Usuario>> GetPaginationAsync(ParametersUsuario parametersUsuario)
        {
            return await usuarioRepository.GetPaginationAsync(parametersUsuario);
        }

        public async Task<ServiceResult<Usuario>> GetByIdAsync(Guid id, bool trackChanges)
        {
            return await usuarioRepository.GetByIdAsync(id, trackChanges);
        }

        public async Task<List<Usuario>> GetListUsuariosByIds(List<Guid> Ids)
        {
            return await usuarioRepository.GetListUsuariosByIds(Ids);
        }

        public async Task<Usuario> AlteraUltimoAcesso(Guid id)
        {
            return await usuarioRepository.AlteraUltimoAcesso(id);
        }

        public async Task<Usuario> PutStatusAsync(Guid id, EStatus status)
        {
            return await usuarioRepository.PutStatusAsync(id, status);
        }

        public bool ValidarId(Guid id)
        {
            return usuarioRepository.ValidarId(id);
        }

        public async Task<bool> VersaoToken(Guid usuarioId)
        {
            return await usuarioRepository.VersaoToken(usuarioId);
        }
    }
}