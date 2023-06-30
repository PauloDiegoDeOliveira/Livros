using Livros.Application.Dtos.Usuario;
using Livros.Application.Dtos.Usuario.Autenticacao;
using Livros.Domain.Enums;
using Livros.Domain.Pagination;

namespace Livros.Application.Interfaces
{
    public interface IUsuarioApplication
    {
        Task<ViewPaginacaoUsuarioDto> GetPaginationAsync(ParametersUsuario parametersUsuario);

        Task<ViewUsuarioDto> GetByIdAsync(Guid id);

        Task<bool> CadastrarUsuario(PostCadastroUsuarioDto postCadastroUsuarioDto);

        Task<ViewUsuarioDto> UpdateUsuario(PutCadastroUsuarioDto putCadastroUsuarioDto);

        Task<ViewLoginDto> Login(PostLoginUsuarioDto postLoginUsuarioDto);

        Task<bool> AlterarSenha(PostAlterarSenhaUsuarioDto postAlterarSenhaUsuarioDto);

        Task<bool> ResetarSenha(PostResetarSenhaDto postResetarSenhaDto);

        Task<bool> EnviarEmailResetarSenha(PostEmailDto postConfirmacaoEmailDto);

        Task<bool> ConfimarEmail(PostConfirmacaoEmailDto postConfirmacaoEmailDto);

        Task<bool> ReenviarConfirmacaoEmail(PostEmailDto postConfirmacaoEmailDto);

        Task<ViewUsuarioDto> DeleteAsync(Guid id);

        Task<ViewUsuarioDto> PutStatusAsync(Guid id, EStatus status);

        bool ValidarId(Guid id);
    }
}