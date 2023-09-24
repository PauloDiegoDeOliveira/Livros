using FluentValidation;
using Livros.Application.Dtos.Idioma;
using Livros.Application.Interfaces;

namespace Livros.Application.Validators.Idioma
{
    public class PostIdiomaValidator : AbstractValidator<PostIdiomaDto>
    {
        private readonly IIdiomaApplication idiomaApplication;

        public PostIdiomaValidator(IIdiomaApplication idiomaApplication)
        {
            this.idiomaApplication = idiomaApplication;

            RuleFor(x => x.Nome)
                .NotEmpty()
                .WithMessage("O campo {PropertyName} não pode ser nulo ou vazio.")
                .Must((dto, nome) => !idiomaApplication.ExisteNomePostDto(dto))
                .When(x => !string.IsNullOrEmpty(x.Nome))
                .WithMessage("Já existe um idioma cadastrado com o nome informado.");
        }
    }
}