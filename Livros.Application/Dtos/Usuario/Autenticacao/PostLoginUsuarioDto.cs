using System.ComponentModel.DataAnnotations;

namespace Livros.Application.Dtos.Usuario.Autenticacao
{
    /// <summary>
    /// Objeto utilizado para inserção.
    /// </summary>
    public class PostLoginUsuarioDto
    {
        /// <summary>
        /// E-mail
        /// </summary>
        /// <example>backend@gmail.com</example>
        [Display(Name = "e-mail antiga")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [EmailAddress(ErrorMessage = "O campo {0 está em um formato inválido.")]
        public string Email { get; set; }

        /// <summary>
        /// Senha
        /// </summary>
        [Display(Name = "senha")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public string Senha { get; set; }
    }
}