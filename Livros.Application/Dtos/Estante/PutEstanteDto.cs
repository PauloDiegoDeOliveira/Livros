using Livros.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Livros.Application.Dtos.Estante
{
    public class PutEstanteDto
    {
        /// <summary>
        /// ObraId
        /// </summary>
        /// <example>085acbb3-a6b5-4cfa-dc22-08daa7d24f76</example>
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public Guid ObraId { get; set; }

        /// <summary>
        /// Id
        /// </summary>
        /// <example>085acbb3-a6b5-4cfa-dc22-08daa7d24f76</example>
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public Guid Id { get; set; }

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

        /// <summary>
        /// Status
        /// </summary>
        [Display(Name = "status")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [EnumDataType(typeof(EStatus), ErrorMessage = "O campo {0} está em formato inválido.")]
        public EStatus Status { get; set; }
    }
}