using System.ComponentModel.DataAnnotations;

namespace Livros.Application.Dtos.Autor
{
    public class ReferenciaAutorDto
    {
        /// <summary>
        /// Id de Autor.
        /// </summary>
        /// <example>3B407CE5-29B6-43E4-B26C-289C7345FEE7</example>
        [Display(Name = "Id de autor.")]
        public Guid AutorId { get; set; }
    }
}