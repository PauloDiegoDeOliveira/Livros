using FluentValidation;
using Livros.Application.Dtos.Volume;
using Livros.Application.Interfaces;

namespace Livros.Application.Validators.Volume
{
    public class PutVolumeValidator : AbstractValidator<PutVolumeDto>
    {
        private readonly IObraApplication obraApplication;

        public PutVolumeValidator(IObraApplication obraApplication)
        {
            this.obraApplication = obraApplication;

            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("O campo {PropertyName} não pode ser nulo ou vazio.")
                .Must(id => obraApplication.ExisteId(id))
                .WithMessage("Nenhum volume foi encontrado com o id informado.");

            RuleFor(x => x.Titulo)
                .NotEmpty()
                .WithMessage("O campo {PropertyName} não pode ser nulo ou vazio.");

            When(x => x.Titulo != null, () =>
            {
                RuleFor(x => x)
                    .Must(dto => !obraApplication.ExisteNomeVolumePutDto(dto))
                    .WithMessage("Já existe um volume cadastrado com o nome informado.");
            });

            RuleFor(x => x.Status)
                .NotEmpty()
                .WithMessage("O campo {PropertyName} não pode ser nulo ou vazio.");
        }
    }
}