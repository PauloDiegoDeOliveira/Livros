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

            RuleFor(x => x.Obras)
                .Must(obras => obras == null || NaoExisteObraIdDuplicados(obras))
                .WithMessage("Não é possível inserir obras repetidas na mesma estante.");

            RuleForEach(x => x.Obras)
                .SetValidator(new ReferenciaObraValidator(obraApplication))
                .When(x => x.Obras != null && x.Obras.Any());
        }

        private static bool NaoExisteObraIdDuplicados(IList<ReferenciaObraDto> obras)
        {
            if (obras == null)
                return true;

            return obras.GroupBy(x => x.Id).All(g => g.Count() == 1);
        }
    }
}