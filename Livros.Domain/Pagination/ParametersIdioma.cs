using Livros.Domain.Enums;

namespace Livros.Domain.Pagination
{
    public class ParametersIdioma : ParametersBase
    {
        public string PalavraChave { get; set; }
        public EOrdenar Ordenar { get; set; }
    }
}