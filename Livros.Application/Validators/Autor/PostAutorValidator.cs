using FluentValidation;
using Livros.Application.Dtos.Autor;
using Livros.Application.Interfaces;

namespace Livros.Application.Validators.Autor
{
    public class PostAutorValidator : AbstractValidator<PostAutorDto>
    {
        private readonly IAutorApplication autorApplication;

        public PostAutorValidator(IAutorApplication autorApplication)
        {
            this.autorApplication = autorApplication;

            RuleFor(x => x.Nome)
                .NotEmpty()
                .WithMessage("O campo {PropertyName} não pode ser nulo ou vazio.")
                .Must((dto, nome) => !autorApplication.ExisteNomePostDto(dto))
                .When(x => !string.IsNullOrEmpty(x.Nome))
                .WithMessage("Já existe um autor cadastrado com o nome informado.");
        }
    }
}