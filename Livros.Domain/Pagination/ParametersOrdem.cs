using Livros.Domain.Enums;

namespace Livros.Domain.Pagination
{
    public class ParametersOrdem : ParametersBase
    {
        public EOrdenar Ordenar { get; set; }
    }
}