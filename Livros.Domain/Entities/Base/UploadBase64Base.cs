namespace Livros.Domain.Entities.Base
{
    public class UploadBase64Base : EntityBase
    {
        public Guid? NomeArquivo { get; set; }
        public string CaminhoRelativo { get; set; }
        public string CaminhoAbsoluto { get; set; }
        public string CaminhoFisico { get; set; }
        public DateTime? HoraEnvio { get; set; }

        protected UploadBase64Base()
        {
            NomeArquivo = Guid.NewGuid();
        }

        public void PolulateInformations(string caminhoFisico, string caminhoAbsoluto, string caminhoRelativo, string extensao)
        {
            CaminhoRelativo = caminhoRelativo + NomeArquivo.ToString() + "." + extensao;
            CaminhoAbsoluto = caminhoAbsoluto + NomeArquivo.ToString() + "." + extensao;
            CaminhoFisico = caminhoFisico + NomeArquivo.ToString() + "." + extensao;
            HoraEnvio = DateTime.Now;
        }

        public void PutInformations(Obra obra)
        {
            NomeArquivo = obra.NomeArquivo;
            CaminhoRelativo = obra.CaminhoRelativo;
            CaminhoAbsoluto = obra.CaminhoAbsoluto;
            CaminhoFisico = obra.CaminhoFisico;
            HoraEnvio = obra.HoraEnvio;
        }
    }
}