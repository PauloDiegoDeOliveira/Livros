using FluentValidation;
using Livros.Application.Dtos.Obra;
using Livros.Application.Interfaces;

namespace Livros.Application.Validators.Obra
{
    public class PutObraValidator : AbstractValidator<PutObraDto>
    {
        private readonly IObraApplication obraApplication;

        public PutObraValidator(IObraApplication obraApplication)
        {
            this.obraApplication = obraApplication;

            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("O campo {PropertyName} não pode ser nulo ou vazio.")
                .Must(id => obraApplication.ExisteId(id))
                .WithMessage("Nenhuma obra foi encontrada com o id informado.");

            RuleFor(x => x.Titulo)
                .NotEmpty()
                .WithMessage("O campo {PropertyName} não pode ser nulo ou vazio.");

            When(x => x.Titulo != null, () =>
            {
                RuleFor(x => x)
                    .Must(dto => !obraApplication.ExisteNomePutDto(dto))
                    .WithMessage("Já existe uma obra cadastrada com o nome informado.");
            });

            RuleFor(x => x.Status)
                .NotEmpty()
                .WithMessage("O campo {PropertyName} não pode ser nulo ou vazio.");
        }
    }
}