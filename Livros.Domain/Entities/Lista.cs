using Livros.Domain.Entities.Base;

namespace Livros.Domain.Entities
{
    public class Lista : EntityBase
    {
        public string UsuarioId { get; set; }
        public string Nome { get; set; }
        public string Publico { get; set; }

        public Usuario Usuario { get; set; }
        public List<Obra> Obras { get; set; }
    }
}