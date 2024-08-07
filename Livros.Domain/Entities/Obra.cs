using Livros.Domain.Entities.Base;

namespace Livros.Domain.Entities
{
    public class Obra : UploadBase64Base
    {
        public string UsuarioId { get; set; }
        public Guid EditoraId { get; set; }
        public string Nome { get; set; }
        public string Anotacao { get; set; }
        public bool Concluido { get; set; }
        public bool VolumeUnico { get; set; }
        public string Tipo { get; set; }

        public Usuario Usuario { get; set; }
        public Editora Editora { get; set; }
        public IList<Estante> Estantes { get; set; }
        public IList<Volume> Volumes { get; set; }
        public IList<Idioma> Idiomas { get; set; }
        public IList<Autor> Autores { get; set; }
        public IList<Genero> Generos { get; set; }

        public void ListaIdiomas(IList<Idioma> idiomas)
        {
            Idiomas = idiomas;
        }

        public void ListaAutores(IList<Autor> autores)
        {
            Autores = autores;
        }

        public void ListaGeneros(IList<Genero> generos)
        {
            Generos = generos;
        }
    }
}