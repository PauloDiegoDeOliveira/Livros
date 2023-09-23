using Livros.Domain.Entities.Base;

namespace Livros.Domain.Entities
{
    public class Estante : EntityBase
    {
        public string UsuarioId { get; set; }
        public string Nome { get; set; }
        public bool Publico { get; set; }

        public Usuario Usuario { get; set; }
        public IList<Obra> Obras { get; set; }
    }
}