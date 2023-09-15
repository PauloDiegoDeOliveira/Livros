using Livros.Domain.Entities.Base;

namespace Livros.Application.Utilities.Image
{
    public class GerenciadorArquivosImagemIFormFile<TEntity> where TEntity : UploadIFormFileBase
    {
        public async Task<bool> CarregarImagemAsync(TEntity uploadForm)
        {
            try
            {
                using FileStream fileStream = new FileStream(uploadForm.CaminhoFisico, FileMode.Create);
                await uploadForm.ImagemUpload.CopyToAsync(fileStream);
            }
            catch (Exception)
            {
                // logger.LogError(ex, "Erro ao carregar a imagem.");
                return false;
            }

            return true;
        }

        public bool DeletarImagem(TEntity uploadForm)
        {
            if (!File.Exists(uploadForm.CaminhoFisico))
            {
                return true;
            }

            try
            {
                File.Delete(uploadForm.CaminhoFisico);
            }
            catch (Exception)
            {
                // logger.LogError(ex, "Erro ao deletar a imagem.");
                return false;
            }

            return true;
        }
    }
}