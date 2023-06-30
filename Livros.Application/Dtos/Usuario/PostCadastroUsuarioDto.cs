using Livros.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Livros.Application.Dtos.Usuario
{
    /// <summary>
    /// Objeto utilizado para inserção.
    /// </summary>
    public class PostCadastroUsuarioDto
    {
        /// <summary>
        /// Nome
        /// </summary>
        /// <example>Back</example>
        [Display(Name = "nome")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(150, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 2)]
        public string Nome { get; set; }

        /// <summary>
        /// Sobrenome
        /// </summary>
        /// <example>End</example>
        [Display(Name = "sobrenome")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(150, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 2)]
        public string Sobrenome { get; set; }

        /// <summary>
        /// E-mail
        /// </summary>
        /// <example>pdiegodo@gmail.com</example>
        [Display(Name = "e-mail")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [EmailAddress(ErrorMessage = "O campo {0} está no formato inválido.")]
        public string Email { get; set; }

        /// <summary>
        /// Gênero
        /// </summary>
        /// <example>Masculino</example>
        [Display(Name = "gênero")]
        public EGenero Genero { get; set; }

        /// <summary>
        /// Data de nascimento
        /// </summary>
        /// <example>1984-05-20</example>
        [Display(Name = "data de nascimento")]
        public DateTime? DataNascimento { get; set; }

        /// <summary>
        /// Senha
        /// </summary>
        [Display(Name = "senha")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(100, ErrorMessage = "O campo senha precisa ter entre {2} e {1} caracteres.", MinimumLength = 6)]
        public string Senha { get; set; }

        /// <summary>
        /// Confirmar senha
        /// </summary>
        [Display(Name = "confirmar senha")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [Compare("Senha", ErrorMessage = "As senhas não conferem.")]
        public string ConfirmarSenha { get; set; }

        /// <summary>
        /// Notificar
        /// </summary>
        [Display(Name = "notificar")]
        public bool Notificar { get; set; }
    }
}