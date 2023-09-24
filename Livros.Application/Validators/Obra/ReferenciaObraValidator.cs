using FluentValidation;
using Livros.Application.Dtos.Obra;
using Livros.Application.Interfaces;

namespace Livros.Application.Validators.Obra
{
    public class ReferenciaObraValidator : AbstractValidator<ReferenciaObraDto>
    {
        private readonly IObraApplication obraApplication;

        public ReferenciaObraValidator(IObraApplication obraApplication)
        {
            this.obraApplication = obraApplication;

            When(x => x.Id != Guid.Empty, () =>
            {
                RuleFor(x => x.Id)
                   .NotEmpty()
                   .WithMessage("O campo {PropertyName} não pode ser nulo ou vazio.")

                .Must((dto, cancelar) =>
                {
                    return ExisteId(dto.Id);
                }).WithMessage("Nenhuma obra foi encontrada com o id informado.");
            });
        }

        private bool ExisteId(Guid id)
        {
            return obraApplication.ExisteId(id);
        }
    }
}