using FluentValidation;
using Livros.Application.Dtos.Idioma;
using Livros.Application.Dtos.Obra;
using Livros.Application.Interfaces;
using Livros.Application.Validators.Idioma;

namespace Livros.Application.Validators.Obra
{
    public class PutObraValidator : AbstractValidator<PutObraDto>
    {
        private readonly IObraApplication obraApplication;
        private readonly IIdiomaApplication idiomaApplication;

        public PutObraValidator(IObraApplication obraApplication,
                                IIdiomaApplication idiomaApplication)
        {
            this.obraApplication = obraApplication;
            this.idiomaApplication = idiomaApplication;

            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("O campo {PropertyName} não pode ser nulo ou vazio.")
                .Must(id => obraApplication.ExisteId(id))
                .WithMessage("Nenhuma obra foi encontrada com o id informado.");

            RuleFor(x => x.Titulo)
                .NotEmpty()
                .WithMessage("O campo {PropertyName} não pode ser nulo ou vazio.");

            When(x => x.Titulo != null, () =>
            {
                RuleFor(x => x)
                    .Must(dto => !obraApplication.ExisteNomePutDto(dto))
                    .WithMessage("Já existe uma obra cadastrada com o nome informado.");
            });

            RuleFor(x => x.Status)
                .NotEmpty()
                .WithMessage("O campo {PropertyName} não pode ser nulo ou vazio.");

            When(x => x.Idiomas != null && x.Idiomas.Any(), () =>
            {
                RuleFor(x => x.Idiomas)
                    .Must(NaoExisteIdiomaIdDuplicados)
                    .WithMessage("Não é possível inserir idiomas repetidos na mesma obra.");

                RuleForEach(x => x.Idiomas)
                    .SetValidator(new ReferenciaIdiomaValidator(idiomaApplication));
            });
        }

        private bool NaoExisteIdiomaIdDuplicados(IList<ReferenciaIdiomaDto> referenciaIdiomaDto)
        {
            return referenciaIdiomaDto?.GroupBy(x => x.Id).All(g => g.Count() == 1) ?? true;
        }
    }
}