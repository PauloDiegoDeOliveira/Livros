using Livros.Domain.Entities.Base;

namespace Livros.Domain.Entities
{
    public class Editora : EntityBase
    {
        public string UsuarioId { get; set; }
        public string Nome { get; set; }

        public Usuario Usuario { get; set; }
        public IList<Obra> Obras { get; set; }
    }
}