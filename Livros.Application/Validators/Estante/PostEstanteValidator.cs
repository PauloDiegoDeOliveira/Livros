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

            RuleFor(x => x)
                .Must(NaoExisteObraIdDuplicados)
                .WithMessage("Não é possível inserir obras repetidas na mesma estante.")
                .When(x => x.Obras != null);

            RuleForEach(x => x.Obras)
                .SetValidator(new ReferenciaObraValidator(obraApplication))
                .When(x => x.Obras != null && x.Obras.Any());
        }

        private static bool NaoExisteObraIdDuplicados(PostEstanteDto postEstanteDto)
        {
            if (postEstanteDto.Obras == null)
                return true;

            return postEstanteDto.Obras.GroupBy(x => x.Id).All(g => g.Count() == 1);
        }
    }
}