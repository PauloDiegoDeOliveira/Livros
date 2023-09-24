using FluentValidation;
using Livros.Application.Dtos.Estante;
using Livros.Application.Interfaces;
using Livros.Application.Validators.Obra;

namespace Livros.Application.Validators.Estante
{
    public class PostEstanteValidator : AbstractValidator<PostEstanteDto>
    {
        private readonly IObraApplication obraApplication;

        public PostEstanteValidator(IObraApplication obraApplication)
        {
            this.obraApplication = obraApplication;

            When(x => x.Obras != null, () =>
            {
                RuleFor(x => x.Obras)
                   .NotEmpty()
                   .WithMessage("O campo {PropertyName} não pode ser nulo ou vazio.");

                RuleFor(x => x)
                    .Must((dto, cancellation) =>
                    {
                        return ExisteObrasDuplicadas(dto);
                    }).WithMessage("Não é possível inserir obras repetidas na mesma estante.");

                RuleForEach(x => x.Obras).SetValidator(new ReferenciaObraValidator(obraApplication));
            });
        }

        private static bool ExisteObrasDuplicadas(PostEstanteDto postEstanteDto)
        {
            return postEstanteDto.Obras.GroupBy(x => x.Id).All(g => g.Count() == 1);
        }
    }
}