using Livros.Domain.Enums;

namespace Livros.Domain.Pagination
{
    public class ParametersObra : ParametersBase
    {
        public Guid AutorId { get; set; }
        public Guid EditoraId { get; set; }
        public Guid GeneroId { get; set; }
        public bool VolumeUnico { get; set; }
        public IList<Guid> IdiomaId { get; set; }
        public ETipo Tipo { get; set; }
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
        public string PalavraChave { get; set; }
        public EOrdenar Ordenar { get; set; }
    }
}