using Livros.Domain.Entities.Base;

namespace Livros.Domain.Entities
{
    public class Obra : EntityBase
    {
        public string UsuarioId { get; set; }
        public Guid IdiomaId { get; set; }
        public Guid ImagemId { get; set; }
        public string Titulo { get; set; }
        public string Anotacao { get; set; }
        public int AvaliacaoTotal { get; set; }
        public decimal PrecoTotal { get; set; }
        public int PaginaTotal { get; set; }
        public bool Concluido { get; set; }
        public bool VolumeUnico { get; set; }
        public string Tipo { get; set; }

        public Usuario Usuario { get; set; }
        public Idioma Idioma { get; set; }
        public Imagem Imagem { get; set; }
        public IList<Editora> Editoras { get; set; }
        public IList<Estante> Estantes { get; set; }
        public IList<Volume> Volumes { get; set; }
        public IList<Autor> Autores { get; set; }
        public IList<Genero> Generos { get; set; }
    }
}