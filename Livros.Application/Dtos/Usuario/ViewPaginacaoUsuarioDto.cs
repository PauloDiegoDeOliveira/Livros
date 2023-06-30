using Livros.Application.Dtos.Pagination;
using Livros.Domain.Pagination;

namespace Livros.Application.Dtos.Usuario
{
    public class ViewPaginacaoUsuarioDto
    {
        public List<ViewUsuarioDto> Pagina { get; set; }
        public ViewPaginationDataDto<Domain.Entities.Usuario> Dados { get; set; }

        public ViewPaginacaoUsuarioDto(PagedList<Domain.Entities.Usuario> pagedList, List<ViewUsuarioDto> list)
        {
            Pagina = list;
            Dados = new ViewPaginationDataDto<Domain.Entities.Usuario>(pagedList);
        }
    }
}