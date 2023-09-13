using Livros.Domain.Enums;

namespace Livros.Application.Dtos.Volume
{
    public class ViewVolumeDto
    {
        public Guid Id { get; set; }
        public Guid ObraId { get; set; }
        public string Titulo { get; set; }
        public int Numero { get; set; }
        public int Paginas { get; set; }
        public string Anotacao { get; set; }
        public decimal Preco { get; set; }
        public int Avaliacao { get; set; }
        public bool Lido { get; set; }
        public DateTime DataLeitura { get; set; }
        public DateTime DataCompra { get; set; }
        public EStatus Status { get; set; }
    }
}