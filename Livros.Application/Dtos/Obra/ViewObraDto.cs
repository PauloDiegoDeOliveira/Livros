using Livros.Application.Dtos.Volume;
using Livros.Domain.Enums;

namespace Livros.Application.Dtos.Obra
{
    public class ViewObraDto
    {
        public Guid Id { get; set; }
        public string UsuarioId { get; set; }
        public string EditoraId { get; set; }
        public string GeneroId { get; set; }
        public string AutorId { get; set; }
        public string Titulo { get; set; }
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
    }
}