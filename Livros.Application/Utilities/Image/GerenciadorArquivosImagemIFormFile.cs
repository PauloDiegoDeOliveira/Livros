using Livros.Domain.Entities.Base;

namespace Livros.Application.Utilities.Image
{
    public class GerenciadorArquivosImagemIFormFile<TEntity> where TEntity : UploadIFormFileBase
    {
        public async Task<bool> CarregarImagemAsync(TEntity entity)
        {
            try
            {
                using FileStream fileStream = new(entity.CaminhoFisico, FileMode.Create);
                await entity.ImagemUpload.CopyToAsync(fileStream);
            }
            catch (Exception)
            {
                // logger.LogError(ex, "Erro ao carregar a imagem.");
                return false;
            }

            return true;
        }

        public bool DeletarImagem(TEntity entity)
        {
            if (!File.Exists(entity.CaminhoFisico))
            {
                return true;
            }

            try
            {
                File.Delete(entity.CaminhoFisico);
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