using Livros.Domain.Enums;

namespace Livros.Domain.Pagination
{
    public class ParametersGenero : ParametersBase
    {
        public string PalavraChave { get; set; }
        public EOrdenar Ordenar { get; set; }
    }
}