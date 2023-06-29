using Livros.Domain.Entities.Base;

namespace Livros.Domain.Entities
{
    public class Genero : EntityBase
    {
        public Guid UsuarioId { get; set; }
        public string Nome { get; set; }
    }
}