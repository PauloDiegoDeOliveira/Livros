using Livros.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Livros.Application.Dtos.Usuario
{
    public class PutCadastroUsuarioDto
    {
        /// <summary>
        /// Id
        /// </summary>
        /// <example>085acbb3-a6b5-4cfa-dc22-08daa7d24f76</example>
        [Display(Name = "id")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public Guid Id { get; set; }

        /// <summary>
        /// Nome
        /// </summary>
        /// <example>Back</example>
        [Display(Name = "nome")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(150, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 2)]
        public string Nome { get; set; }

        /// <summary>
        /// E-mail
        /// </summary>
        /// <example>teste@teste.com</example>
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
        /// Notificar
        /// </summary>
        [Display(Name = "notificar")]
        public bool Notificar { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        [Display(Name = "status")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [EnumDataType(typeof(EStatus), ErrorMessage = "O campo {0} está em formato inválido.")]
        public EStatus Status { get; set; }
    }
}