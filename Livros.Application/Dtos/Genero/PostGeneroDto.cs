using System.ComponentModel.DataAnnotations;

namespace Livros.Application.Dtos.Genero
{
    public class PostGeneroDto
    {
        /// <summary>
        /// ObraId
        /// </summary>
        /// <example>085acbb3-a6b5-4cfa-dc22-08daa7d24f76</example>
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public Guid ObraId { get; set; }

        /// <summary>
        /// Nome
        /// </summary>
        /// <example>Ficção</example>
        [Display(Name = "nome")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(150, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 2)]
        public string Nome { get; set; }
    }
}