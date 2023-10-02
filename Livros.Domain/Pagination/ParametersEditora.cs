using Livros.Domain.Enums;

namespace Livros.Domain.Pagination
{
    public class ParametersEditora : ParametersBase
    {
        public string PalavraChave { get; set; }
        public EOrdenar Ordenar { get; set; }
        public EQuantidadeObras QuantidadeObras { get; set; }
    }
}