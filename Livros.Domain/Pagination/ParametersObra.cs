using Livros.Domain.Enums;

namespace Livros.Domain.Pagination
{
    public class ParametersObra : ParametersBase
    {
        public string PalavraChave { get; set; }
        public EOrdenar Ordenar { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
    }
}