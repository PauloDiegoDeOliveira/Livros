using System.ComponentModel.DataAnnotations;

namespace Livros.Application.Dtos.Idioma
{
    public class PostIdiomaDto
    {
        /// <summary>
        /// Nome
        /// </summary>
        /// <example>Inglês</example>
        [Display(Name = "nome")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(150, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 2)]
        public string Nome { get; set; }

        /// <summary>
        /// Alias
        /// </summary>
        /// <example>Inglês</example>
        [Display(Name = "alias")]
        public string Alias { get; set; }
    }
}