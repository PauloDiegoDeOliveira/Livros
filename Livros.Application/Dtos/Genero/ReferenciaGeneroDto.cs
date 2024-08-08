using System.ComponentModel.DataAnnotations;

namespace Livros.Application.Dtos.Genero
{
    public class ReferenciaGeneroDto
    {
        /// <summary>
        /// Id de Gênero.
        /// </summary>
        /// <example>3B407CE5-29B6-43E4-B26C-289C7345FEE7</example>
        [Display(Name = "Id do gênero.")]
        public Guid Id { get; set; }
    }
}