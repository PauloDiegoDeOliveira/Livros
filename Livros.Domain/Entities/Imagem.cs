using Livros.Domain.Entities.Base;

namespace Livros.Domain.Entities
{
    public class Imagem : UploadIFormFileBase
    {
        public string TipoUpload { get; set; }

        //public List<Livro> Livros { get; set; }
    }
}