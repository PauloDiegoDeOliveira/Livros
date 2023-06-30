using System.ComponentModel.DataAnnotations;

namespace Livros.Application.Dtos.Usuario.Autenticacao
{
    /// <summary>
    /// Objeto utilizado para inserção.
    /// </summary>
    public class PostConfirmacaoEmailDto
    {
        /// <summary>
        /// Id do Usuário
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
    }
}