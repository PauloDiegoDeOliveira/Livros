using Livros.Domain.Entities.Base;

namespace Livros.Domain.Entities
{
    public class Idioma : EntityBase
    {
        public string Nome { get; set; }
        public string Alias { get; set; }

        public IList<Obra> Obras { get; set; }
    }
}