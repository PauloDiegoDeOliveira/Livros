using Livros.Domain.Entities.Base;

namespace Livros.Application.Utilities.Image
{
    public class UploadIFormFileMethods<TEntity> where TEntity : UploadIFormFileBase
    {
        public async Task<bool> UploadImage(TEntity uploadForm)
        {
            try
            {
                using var fileStream = new FileStream(uploadForm.CaminhoFisico, FileMode.Create);
                await uploadForm.ImagemUpload.CopyToAsync(fileStream);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteImage(TEntity uploadForm)
        {
            if (!File.Exists(uploadForm.CaminhoFisico))
            {
                return true;
            }

            try
            {
                File.Delete(uploadForm.CaminhoFisico);
                await Task.CompletedTask;

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}