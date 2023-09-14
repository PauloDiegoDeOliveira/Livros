namespace Livros.Domain.Entities.Base
{
    public class UploadBase64Base : EntityBase
    {
        public Guid NomeArquivo { get; private set; }
        public string CaminhoRelativo { get; private set; }
        public string CaminhoAbsoluto { get; private set; }
        public string CaminhoFisico { get; private set; }
        public DateTime HoraEnvio { get; private set; }

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
            CaminhoRelativo = obra.CaminhoRelativo;
            CaminhoAbsoluto = obra.CaminhoAbsoluto;
            CaminhoFisico = obra.CaminhoFisico;
            HoraEnvio = obra.HoraEnvio;
        }
    }
}