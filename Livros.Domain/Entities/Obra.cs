using Livros.Domain.Entities.Base;

namespace Livros.Domain.Entities
{
    public class Obra : UploadBase64Base
    {
        public string UsuarioId { get; set; }
        public Guid EditoraId { get; set; }
        public Guid GeneroId { get; set; }
        public Guid AutorId { get; set; }
        public string Titulo { get; set; }
        public string Anotacao { get; set; }
        public int? AvaliacaoTotal { get; set; }
        public decimal? PrecoTotal { get; set; }
        public int? PaginaTotal { get; set; }
        public bool Concluido { get; set; }
        public bool VolumeUnico { get; set; }
        public string Tipo { get; set; }

        public Usuario Usuario { get; set; }
        public Editora Editora { get; set; }
        public Genero Genero { get; set; }
        public Autor Autor { get; set; }
        public IList<Idioma> Idiomas { get; set; }
        public IList<Estante> Estantes { get; set; }
        public IList<Volume> Volumes { get; set; }

        public void ListaIdiomas(IList<Idioma> idiomas)
        {
            Idiomas = idiomas;
        }
    }
}