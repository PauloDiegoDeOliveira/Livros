using System.ComponentModel.DataAnnotations;

namespace Livros.Application.Dtos.Idioma
{
    public class ReferenciaIdiomaDto
    {
        /// <summary>
        /// Id de idioma.
        /// </summary>
        /// <example>3B407CE5-29B6-43E4-B26C-289C7345FEE7</example>
        [Display(Name = "Id de idioma.")]
        public Guid Id { get; set; }
    }
}