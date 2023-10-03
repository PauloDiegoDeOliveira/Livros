using Livros.Domain.Enums;

namespace Livros.Application.Dtos.Autor
{
    public class ViewAutorDto
    {
        public string UsuarioId { get; set; }
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public int QuantidadeObras { get; set; }
        public EStatus Status { get; set; }
    }
}