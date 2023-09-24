using FluentValidation;
using Livros.Application.Dtos.Autor;
using Livros.Application.Interfaces;

namespace Livros.Application.Validators.Autor
{
    public class PutAutorValidator : AbstractValidator<PutAutorDto>
    {
        private readonly IAutorApplication autorApplication;

        public PutAutorValidator(IAutorApplication autorApplication)
        {
            this.autorApplication = autorApplication;

            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("O campo {PropertyName} não pode ser nulo ou vazio.")
                .Must(id => autorApplication.ExisteId(id))
                .WithMessage("Nenhum autor foi encontrado com o id informado.");

            RuleFor(x => x.Nome)
                .NotEmpty()
                .WithMessage("O campo {PropertyName} não pode ser nulo ou vazio.");

            When(x => x.Nome != null, () =>
            {
                RuleFor(x => x)
                    .Must(dto => !autorApplication.ExisteNomePutDto(dto))
                    .WithMessage("Já existe um autor cadastrado com o nome informado.");
            });

            RuleFor(x => x.Status)
                .NotEmpty()
                .WithMessage("O campo {PropertyName} não pode ser nulo ou vazio.");
        }
    }
}