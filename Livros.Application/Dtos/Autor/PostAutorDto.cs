using Livros.Domain.Enums;

namespace Livros.Application.Dtos.Autor
{
    public class PostAutorDto
    {
        public Guid ObraId { get; set; }
        public string Nome { get; set; }
        public EStatus Status { get; set; }
    }
}