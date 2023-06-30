using Livros.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Livros.Application.Dtos.Base
{
    /// <summary>
    /// Objeto utilizado para alteração de status.
    /// </summary>
    public class PutStatusDto
    {
        /// <summary>
        /// Id
        /// </summary>
        /// <example>876777D9-EE18-4798-D3AE-08DA85F40146</example>
        [Display(Name = "Id.")]
        [Required(ErrorMessage = "O campo id é obrigatório.")]
        [RegularExpression(@"^[{]?[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}[}]?$", ErrorMessage = "O Id está em um formato inválido.")]
        public Guid Id { get; set; }

        /// <summary>
        /// Status para qual será alterado
        /// </summary>
        /// <example>Ativo</example>
        [Display(Name = "Status.")]
        [Required(ErrorMessage = "O campo status é obrigatório.")]
        [EnumDataType(typeof(EStatus))]
        public EStatus Status { get; set; }

        public PutStatusDto()
        {
        }

        public PutStatusDto(Guid id, EStatus eStatus)
        {
            Id = id;
            Status = eStatus;
        }
    }
}