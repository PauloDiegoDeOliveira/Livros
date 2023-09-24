using FluentValidation;
using Livros.Application.Dtos.Estante;
using Livros.Application.Dtos.Obra;
using Livros.Application.Interfaces;
using Livros.Application.Validators.Obra;

namespace Livros.Application.Validators.Estante
{
    public class PutEstanteValidator : AbstractValidator<PutEstanteDto>
    {
        private readonly IEstanteApplication estanteApplication;
        private readonly IObraApplication obraApplication;

        public PutEstanteValidator(IEstanteApplication estanteApplication,
                                   IObraApplication obraApplication)
        {
            this.estanteApplication = estanteApplication;
            this.obraApplication = obraApplication;

            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("O campo {PropertyName} não pode ser nulo ou vazio.")
                .Must(id => estanteApplication.ExisteId(id))
                .WithMessage("Nenhuma estante foi encontrada com o id informado.");

            RuleFor(x => x.Status)
                .NotEmpty()
                .WithMessage("O campo {PropertyName} não pode ser nulo ou vazio.");

            When(x => x.Obras != null && x.Obras.Any(), () =>
            {
                RuleFor(x => x.Obras)
                    .Must(NaoExisteObraIdDuplicados)
                    .WithMessage("Não é possível inserir obras repetidas na mesma estante.");

                RuleForEach(x => x.Obras)
                    .SetValidator(new ReferenciaObraValidator(obraApplication));
            });
        }

        private bool NaoExisteObraIdDuplicados(IList<ReferenciaObraDto> referenciaObraDto)
        {
            return referenciaObraDto?.GroupBy(x => x.Id).All(g => g.Count() == 1) ?? true;
        }
    }
}