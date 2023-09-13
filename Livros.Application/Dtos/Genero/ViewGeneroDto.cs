using Livros.Domain.Enums;

namespace Livros.Application.Dtos.Genero
{
    public class ViewGeneroDto
    {
        public Guid Id { get; set; }
        public string UsuarioId { get; set; }
        public Guid ObraId { get; set; }
        public string Nome { get; set; }
        public EStatus Status { get; set; }
    }
}