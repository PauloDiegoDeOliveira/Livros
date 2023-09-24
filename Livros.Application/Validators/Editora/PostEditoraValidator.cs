using FluentValidation;
using Livros.Application.Dtos.Editora;
using Livros.Application.Interfaces;

namespace Livros.Application.Validators.Editora
{
    public class PostEditoraValidator : AbstractValidator<PostEditoraDto>
    {
        private readonly IEditoraApplication editoraApplication;

        public PostEditoraValidator(IEditoraApplication editoraApplication)
        {
            this.editoraApplication = editoraApplication;

            RuleFor(x => x.Nome)
                .NotEmpty()
                .WithMessage("O campo {PropertyName} não pode ser nulo ou vazio.")
                .Must((dto, nome) => !editoraApplication.ExisteNomePostDto(dto))
                .When(x => !string.IsNullOrEmpty(x.Nome))
                .WithMessage("Já existe uma editora cadastrada com o nome informado.");
        }
    }
}