using Livros.Domain.Entities.Base;

namespace Livros.Domain.Entities
{
    public class Obra : EntityBase
    {
        public string Titulo { get; set; }
        public string Anotacao { get; set; }
        public int AvaliacaoTotal { get; set; }
        public decimal PrecoTotal { get; set; }
        public decimal PaginaTotal { get; set; }
        public bool Concluido { get; set; }
        public bool VolumeUnico { get; set; }
        public string Tipo { get; set; }
    }
}