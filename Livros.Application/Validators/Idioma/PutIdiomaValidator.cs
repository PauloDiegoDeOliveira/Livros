using FluentValidation;
using Livros.Application.Dtos.Idioma;
using Livros.Application.Interfaces;

namespace Livros.Application.Validators.Idioma
{
    public class PutIdiomaValidator : AbstractValidator<PutIdiomaDto>
    {
        private readonly IIdiomaApplication IIdiomaApplication;

        public PutIdiomaValidator(IIdiomaApplication IIdiomaApplication)
        {
            this.IIdiomaApplication = IIdiomaApplication;

            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("O campo {PropertyName} não pode ser nulo ou vazio.")
                .Must(id => IIdiomaApplication.ExisteId(id))
                .WithMessage("Nenhum idioma foi encontrado com o id informado.");

            RuleFor(x => x.Nome)
                .NotEmpty()
                .WithMessage("O campo {PropertyName} não pode ser nulo ou vazio.");

            When(x => x.Nome != null, () =>
            {
                RuleFor(x => x)
                    .Must(dto => !IIdiomaApplication.ExisteNomePutDto(dto))
                    .WithMessage("Já existe um idioma cadastrado com o nome informado.");
            });

            RuleFor(x => x.Status)
                .NotEmpty()
                .WithMessage("O campo {PropertyName} não pode ser nulo ou vazio.");
        }
    }
}