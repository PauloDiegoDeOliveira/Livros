using FluentValidation;
using Livros.Application.Dtos.Volume;
using Livros.Application.Interfaces;

namespace Livros.Application.Validators.Volume
{
    public class PostVolumeValidator : AbstractValidator<PostVolumeDto>
    {
        private readonly IObraApplication obraApplication;

        public PostVolumeValidator(IObraApplication obraApplication)
        {
            this.obraApplication = obraApplication;
        }
    }
}