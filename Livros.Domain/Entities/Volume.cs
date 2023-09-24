using Livros.Domain.Entities.Base;

namespace Livros.Domain.Entities
{
    public class Volume : EntityBase
    {
        public Guid ObraId { get; set; }
        public string Nome { get; set; }
        public int Ordem { get; set; }
        public int Paginas { get; set; }
        public string Anotacao { get; set; }
        public decimal Preco { get; set; }
        public int Avaliacao { get; set; }
        public bool Lido { get; set; }
        public DateTime DataLeitura { get; set; }
        public DateTime DataCompra { get; set; }

        public Obra Obra { get; set; }
    }
}