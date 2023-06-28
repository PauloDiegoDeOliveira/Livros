using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;

namespace Livros.Domain.Entities.Base
{
    public class UploadIFormFileBase : EntityBase
    {
        [NotMapped]
        public IFormFile ImagemUpload { get; set; }

        public Guid NomeArquivoBanco { get; set; }
        public long TamanhoEmBytes { get; set; }
        public string ContentType { get; set; }
        public string ExtensaoArquivo { get; set; }
        public string NomeArquivoOriginal { get; set; }
        public string CaminhoRelativo { get; set; }
        public string CaminhoAbsoluto { get; set; }
        public string CaminhoFisico { get; set; }
        public DateTime HoraEnvio { get; set; }

        protected UploadIFormFileBase()
        { }

        public void PolulateInformations(UploadIFormFileBase uploadForm, string caminhoFisico, string caminhoAbsoluto, string caminhoRelativo)
        {
            NomeArquivoBanco = Guid.NewGuid();
            ImagemUpload = uploadForm.ImagemUpload;
            TamanhoEmBytes = uploadForm.ImagemUpload.Length;
            ContentType = uploadForm.ImagemUpload.ContentType;
            ExtensaoArquivo = Path.GetExtension(uploadForm.ImagemUpload.FileName);
            NomeArquivoOriginal = Path.GetFileNameWithoutExtension(uploadForm.ImagemUpload.FileName);
            CaminhoRelativo = caminhoRelativo + NomeArquivoBanco + ExtensaoArquivo;
            CaminhoAbsoluto = caminhoAbsoluto + NomeArquivoBanco + ExtensaoArquivo;
            CaminhoFisico = caminhoFisico + NomeArquivoBanco + ExtensaoArquivo;
            HoraEnvio = DateTime.Now;
        }

        public void PutInformations(UploadIFormFileBase uploadForm)
        {
            NomeArquivoBanco = uploadForm.NomeArquivoBanco;
            ImagemUpload = uploadForm.ImagemUpload;
            TamanhoEmBytes = uploadForm.TamanhoEmBytes;
            ContentType = uploadForm.ContentType;
            ExtensaoArquivo = uploadForm.ExtensaoArquivo;
            NomeArquivoOriginal = uploadForm.NomeArquivoOriginal;
            CaminhoRelativo = uploadForm.CaminhoRelativo;
            CaminhoAbsoluto = uploadForm.CaminhoAbsoluto;
            CaminhoFisico = uploadForm.CaminhoFisico;
            HoraEnvio = uploadForm.HoraEnvio;
        }
    }
}