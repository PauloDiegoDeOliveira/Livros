using System.ComponentModel.DataAnnotations;

namespace Livros.Application.Dtos.Obra
{
    public class ReferenciaObraDto
    {
        /// <summary>
        /// Id da obra.
        /// </summary>
        /// <example>3B407CE5-29B6-43E4-B26C-289C7345FEE7</example>
        [Display(Name = "Id da obra.")]
        public Guid Id { get; set; }
    }
}