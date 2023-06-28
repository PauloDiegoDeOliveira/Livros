using Livros.Domain.Entities.Base;

namespace Livros.Domain.Entities
{
    public class Upload : UploadIFormFileBase
    {
        public string TipoUpload { get; set; }

        //public List<Livro> Livros { get; set; }
    }
}