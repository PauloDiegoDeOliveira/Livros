using FluentValidation;
using Livros.Application.Dtos.Obra;
using Livros.Application.Interfaces;
using Livros.Application.Validators.Idioma;

namespace Livros.Application.Validators.Obra
{
    public class PostObraValidator : AbstractValidator<PostObraDto>
    {
        private readonly IObraApplication obraApplication;
        private readonly IIdiomaApplication idiomaApplication;

        public PostObraValidator(IObraApplication obraApplication,
                                 IIdiomaApplication idiomaApplication)
        {
            this.obraApplication = obraApplication;
            this.idiomaApplication = idiomaApplication;

            RuleFor(x => x.Titulo)
                  .NotEmpty()
                  .WithMessage("O campo {PropertyName} não pode ser nulo ou vazio.")
                  .Must((dto, nome) => !obraApplication.ExisteNomePostDto(dto))
                  .When(x => !string.IsNullOrEmpty(x.Titulo))
                  .WithMessage("Já existe uma obra cadastrada com o nome informado.");

            When(x => x.Idiomas != null && x.Idiomas.Any(), () =>
            {
                RuleFor(x => x)
                    .Must(NaoExisteIdiomaIdDuplicados)
                    .WithMessage("Não é possível inserir idiomas repetidos na mesma obra.");

                RuleForEach(x => x.Idiomas)
                    .SetValidator(new ReferenciaIdiomaValidator(idiomaApplication));
            });
        }

        private bool NaoExisteIdiomaIdDuplicados(PostObraDto postObraDto)
        {
            return postObraDto.Idiomas?.GroupBy(x => x.Id).All(g => g.Count() == 1) ?? true;
        }
    }
}