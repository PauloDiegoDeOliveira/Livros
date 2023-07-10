using Livros.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Livros.Application.Dtos.Obra
{
    public class PutObraDto : PostObraDto
    {
        /// <summary>
        /// Id
        /// </summary>
        /// <example>085acbb3-a6b5-4cfa-dc22-08daa7d24f76</example>
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public Guid Id { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        [Display(Name = "status")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [EnumDataType(typeof(EStatus), ErrorMessage = "O campo {0} está em formato inválido.")]
        public EStatus Status { get; set; }
    }
}