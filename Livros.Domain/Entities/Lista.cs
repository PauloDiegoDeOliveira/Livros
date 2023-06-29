using Livros.Domain.Entities.Base;

namespace Livros.Domain.Entities
{
    public class Lista : EntityBase
    {
        public Guid UsuarioId { get; set; }
        public string Nome { get; set; }
        public string Publico { get; set; }
    }
}