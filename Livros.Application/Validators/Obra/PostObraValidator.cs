using FluentValidation;
using Livros.Application.Dtos.Obra;
using Livros.Application.Interfaces;

namespace Livros.Application.Validators.Obra
{
    public class PostObraValidator : AbstractValidator<PostObraDto>
    {
        private readonly IObraApplication obraApplication;

        public PostObraValidator(IObraApplication obraApplication)
        {
            this.obraApplication = obraApplication;

            RuleFor(x => x.Titulo)
                  .NotEmpty()
                  .WithMessage("O campo {PropertyName} não pode ser nulo ou vazio.")
                  .Must((dto, nome) => !obraApplication.ExisteNomePostDto(dto))
                  .When(x => !string.IsNullOrEmpty(x.Titulo))
                  .WithMessage("Já existe uma obra cadastrada com o nome informado.");
        }
    }
}