using Livros.Application.Dtos.Autor;
using Livros.Application.Dtos.Editora;
using Livros.Application.Dtos.Genero;
using Livros.Application.Dtos.Lista;
using Livros.Application.Dtos.Volume;
using Livros.Domain.Enums;

namespace Livros.Application.Dtos.Obra
{
    public class ViewObraDetalhesDto
    {
        public Guid Id { get; set; }
        public string UsuarioId { get; set; }
        public Guid EditoraId { get; set; }
        public Guid GeneroId { get; set; }
        public Guid AutorId { get; set; }
        public string Nome { get; set; }
        public string Anotacao { get; set; }
        public int AvaliacaoTotal { get; set; }
        public string PrecoTotal { get; set; }
        public int PaginaTotal { get; set; }
        public bool Concluido { get; set; }
        public bool VolumeUnico { get; set; }
        public string CaminhoAbsoluto { get; set; }
        public string CaminhoRelativo { get; set; }
        public ETipo Tipo { get; set; }
        public EStatus Status { get; set; }

        public IList<ViewVolumeDto> Volumes { get; set; }
        public ViewEditoraDto Editora { get; set; }
        public ViewGeneroDto Genero { get; set; }
        public ViewAutorDto Autor { get; set; }
        public IList<ViewEstanteDto> Estantes { get; set; }
    }
}