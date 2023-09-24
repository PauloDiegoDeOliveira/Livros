using FluentValidation;
using Livros.Application.Dtos.Editora;
using Livros.Application.Interfaces;

namespace Livros.Application.Validators.Editora
{
    public class PutEditoraValidator : AbstractValidator<PutEditoraDto>
    {
        private readonly IEditoraApplication editoraApplication;

        public PutEditoraValidator(IEditoraApplication editoraApplication)
        {
            this.editoraApplication = editoraApplication;

            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("O campo {PropertyName} não pode ser nulo ou vazio.")
                .Must(id => editoraApplication.ExisteId(id))
                .WithMessage("Nenhuma editora foi encontrada com o id informado.");

            RuleFor(x => x.Nome)
                .NotEmpty()
                .WithMessage("O campo {PropertyName} não pode ser nulo ou vazio.");

            When(x => x.Nome != null, () =>
            {
                RuleFor(x => x)
                    .Must(dto => !editoraApplication.ExisteNomePutDto(dto))
                    .WithMessage("Já existe uma editora cadastrada com o nome informado.");
            });

            RuleFor(x => x.Status)
                .NotEmpty()
                .WithMessage("O campo {PropertyName} não pode ser nulo ou vazio.");
        }
    }
}