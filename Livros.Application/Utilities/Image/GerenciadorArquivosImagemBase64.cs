using Livros.Domain.Entities.Base;

namespace Livros.Application.Utilities.Image
{
    public class GerenciadorArquivosImagemBase64<TEntity> where TEntity : UploadBase64Base
    {
        public async Task CarregarDeBase64Async(string caminho, string base64string)
        {
            string caminhoArquivo = Path.Combine(Directory.GetCurrentDirectory(), caminho);
            byte[] dadosImagem = Convert.FromBase64String(base64string);
            await File.WriteAllBytesAsync(caminhoArquivo, dadosImagem);
        }

        public void DeletarImagem(TEntity uploadB64)
        {
            if (File.Exists(uploadB64.CaminhoFisico))
            {
                File.Delete(uploadB64.CaminhoFisico);
            }
        }
    }
}