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
                .NotNull()
                .WithMessage("O campo {PropertyName} não pode ser nulo.")

                .NotEmpty()
                .WithMessage("O campo {PropertyName} não pode ser vazio.");

            When(x => x.Nome != null, () =>
            {
                RuleFor(x => x)
                    .Must((dto, cancellation) =>
                    {
                        return !ExisteNomePostDto(dto);
                    }).WithMessage("Já existe um autor cadastrado com o nome informado.");
            });
        }

        private bool ExisteNomePostDto(PostAutorDto postAutorDto)
        {
            return autorApplication.ExisteNomePostDto(postAutorDto);
        }
    }
}