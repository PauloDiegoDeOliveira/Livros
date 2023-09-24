using FluentValidation;
using Livros.Application.Dtos.Genero;
using Livros.Application.Interfaces;

namespace Livros.Application.Validators.Genero
{
    public class PutGeneroValidator : AbstractValidator<PutGeneroDto>
    {
        private readonly IGeneroApplication generoApplication;

        public PutGeneroValidator(IGeneroApplication generoApplication)
        {
            this.generoApplication = generoApplication;

            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("O campo {PropertyName} não pode ser nulo ou vazio.")
                .Must(id => generoApplication.ExisteId(id))
                .WithMessage("Nenhum gênero foi encontrado com o id informado.");

            RuleFor(x => x.Nome)
                .NotEmpty()
                .WithMessage("O campo {PropertyName} não pode ser nulo ou vazio.");

            When(x => x.Nome != null, () =>
            {
                RuleFor(x => x)
                    .Must(dto => !generoApplication.ExisteNomePutDto(dto))
                    .WithMessage("Já existe um gênero cadastrado com o nome informado.");
            });

            RuleFor(x => x.Status)
                .NotEmpty()
                .WithMessage("O campo {PropertyName} não pode ser nulo ou vazio.");
        }
    }
}