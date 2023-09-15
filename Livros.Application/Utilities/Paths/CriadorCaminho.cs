using Livros.Domain.Enums;

namespace Livros.Application.Utilities.Paths
{
    public static class CriadorCaminho
    {
        public static string CriarCaminhoPorIp(string caminhoIp)
        {
            string caminho = MontarCaminhoData(caminhoIp, @"\");

            if (!Directory.Exists(caminho))
            {
                Directory.CreateDirectory(caminho);
            }

            return caminho;
        }

        public static string CriarCaminhoAbsoluto(string caminhoAbsoluto)
        {
            return MontarCaminhoData(caminhoAbsoluto, "/");
        }

        public static string CriarCaminhoRelativo(string caminhoRelativo, string ultimaParte)
        {
            string[] partes = caminhoRelativo.Split(new[] { ultimaParte }, 2, StringSplitOptions.RemoveEmptyEntries);
            return partes.Length > 1 ? partes[1] : string.Empty;
        }

        private static string MontarCaminhoData(string caminhoExterno, string separador)
        {
            return string.Join(separador,
                               caminhoExterno,
                               ExtrairData.ExtrairComponente(EData.Year),
                               ExtrairData.ExtrairComponente(EData.Month),
                               ExtrairData.ExtrairComponente(EData.Day),
                               "" // Adicionado para colocar um separador adicional após a data
                               );
        }
    }
}