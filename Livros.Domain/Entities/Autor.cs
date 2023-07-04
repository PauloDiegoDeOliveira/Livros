using Livros.Domain.Entities.Base;

namespace Livros.Domain.Entities
{
    public class Autor : EntityBase
    {
        public Guid UsuarioId { get; set; }
        public Guid ObraId { get; set; }
        public string Nome { get; set; }

        public Usuario Usuario { get; set; }
        public Obra Obra { get; set; }
    }
}