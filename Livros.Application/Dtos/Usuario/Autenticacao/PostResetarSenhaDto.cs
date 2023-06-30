using System.ComponentModel.DataAnnotations;

namespace Livros.Application.Dtos.Usuario.Autenticacao
{
    /// <summary>
    /// Objeto utilizado para inserção.
    /// </summary>
    public class PostResetarSenhaDto
    {
        /// <summary>
        /// Id usuário
        /// </summary>
        /// <example>876777D9-EE18-4798-D3AE-08DA85F40146</example>
        [Display(Name = "id usuário")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [RegularExpression(@"^[{]?[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}[}]?$", ErrorMessage = "O campo {0} está em um formato inválido.")]
        public Guid UsuarioId { get; set; }

        /// <summary>
        /// Token
        /// </summary>
        [Display(Name = "token")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public string Token { get; set; }

        /// <summary>
        /// Nova senha
        /// </summary>
        [Display(Name = "nova senha")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(100, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres.", MinimumLength = 6)]
        public string NovaSenha { get; set; }

        /// <summary>
        /// Confirmar senha
        /// </summary>
        [Display(Name = "confirmar senha")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [Compare("NovaSenha", ErrorMessage = "As senhas não conferem.")]
        public string ConfirmarSenha { get; set; }
    }
}