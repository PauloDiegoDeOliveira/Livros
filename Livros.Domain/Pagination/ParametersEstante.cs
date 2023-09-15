using Livros.Domain.Enums;

namespace Livros.Domain.Pagination
{
    public class ParametersEstante : ParametersBase
    {
        public string PalavraChave { get; set; }
        public EOrdenar Ordenar { get; set; }
    }
}