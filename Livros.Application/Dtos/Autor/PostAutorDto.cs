using System.ComponentModel.DataAnnotations;

namespace Livros.Application.Dtos.Autor
{
    public class PostAutorDto
    {
        /// <summary>
        /// Nome
        /// </summary>
        /// <example>Isaac Asimov</example>
        [Display(Name = "nome")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(150, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 2)]
        public string Nome { get; set; }
    }
}