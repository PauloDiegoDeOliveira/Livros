using System.ComponentModel.DataAnnotations;

namespace Livros.Application.Dtos.Estante
{
    public class PostEstanteDto
    {
        /// <summary>
        /// Nome
        /// </summary>
        /// <example>Melhores do ano de 2023</example>
        [Display(Name = "nome")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(150, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 2)]
        public string Nome { get; set; }

        /// <summary>
        /// Público
        /// </summary>
        [Display(Name = "público")]
        public bool Publico { get; set; }
    }
}