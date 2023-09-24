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

            RuleFor(x => x.Titulo)
                .NotEmpty()
                .WithMessage("O campo {PropertyName} não pode ser nulo ou vazio.")
                .Must((dto, nome) => !obraApplication.ExisteNomeNumeroVolumePostDto(dto))
                .When(x => !string.IsNullOrEmpty(x.Titulo))
                .WithMessage("Já existe um volume cadastrado com o numero informado.");
        }
    }
}