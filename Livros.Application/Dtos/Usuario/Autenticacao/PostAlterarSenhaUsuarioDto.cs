using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Livros.Application.Dtos.Usuario.Autenticacao
{
    /// <summary>
    /// Objeto utilizado para inserção.
    /// </summary>
    public class PostAlterarSenhaUsuarioDto
    {
        [JsonIgnore]
        public Guid UsuarioId { get; set; }

        /// <summary>
        /// Senha Antiga
        /// </summary>
        [Display(Name = "senha antiga")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public string SenhaAntiga { get; set; }

        /// <summary>
        /// Nova Senha
        /// </summary>
        [Display(Name = "nova senha")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public string NovaSenha { get; set; }

        /// <summary>
        /// Confirmar Senha
        /// </summary>
        [Display(Name = "confirmar senha")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [Compare("NovaSenha", ErrorMessage = "As senhas não conferem.")]
        public string ConfirmarSenha { get; set; }
    }
}