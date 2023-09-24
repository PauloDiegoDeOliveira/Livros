using FluentValidation;
using Livros.Application.Dtos.Idioma;
using Livros.Application.Interfaces;

namespace Livros.Application.Validators.Idioma
{
    public class ReferenciaIdiomaValidator : AbstractValidator<ReferenciaIdiomaDto>
    {
        private readonly IIdiomaApplication idiomaApplication;

        public ReferenciaIdiomaValidator(IIdiomaApplication idiomaApplication)
        {
            this.idiomaApplication = idiomaApplication;

            When(x => x.Id != Guid.Empty, () =>
            {
                RuleFor(x => x.Id)
                   .NotEmpty()
                   .WithMessage("O campo {PropertyName} não pode ser nulo ou vazio.")
                   .Must(id => idiomaApplication.ExisteId(id))
                   .WithMessage("Nenhum idioma foi encontrado com o id informado.");
            });
        }
    }
}