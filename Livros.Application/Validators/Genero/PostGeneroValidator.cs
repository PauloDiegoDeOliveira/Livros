using FluentValidation;
using Livros.Application.Dtos.Genero;
using Livros.Application.Interfaces;

namespace Livros.Application.Validators.Genero
{
    public class PostGeneroValidator : AbstractValidator<PostGeneroDto>
    {
        private readonly IGeneroApplication generoApplication;

        public PostGeneroValidator(IGeneroApplication generoApplication)
        {
            this.generoApplication = generoApplication;

            RuleFor(x => x.Nome)
                .NotEmpty()
                .WithMessage("O campo {PropertyName} não pode ser nulo ou vazio.")
                .Must((dto, nome) => !generoApplication.ExisteNomePostDto(dto))
                .When(x => !string.IsNullOrEmpty(x.Nome))
                .WithMessage("Já existe um gênero cadastrado com o nome informado.");
        }
    }
}