using Livros.Domain.Entities.Base;

namespace Livros.Domain.Entities
{
    public class Editora : EntityBase
    {
        public Guid UsuarioId { get; set; }
        public string Nome { get; set; }
    }
}