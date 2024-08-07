using FluentValidation;
using Livros.Application.Dtos.Obra;
using Livros.Application.Interfaces;
using Livros.Application.Validators.Idioma;

namespace Livros.Application.Validators.Obra
{
    public class PostObraValidator : AbstractValidator<PostObraDto>
    {
        private readonly IObraApplication obraApplication;
        private readonly IEditoraApplication editoraApplication;
        private readonly IGeneroApplication generoApplication;
        private readonly IAutorApplication autorApplication;
        private readonly IIdiomaApplication idiomaApplication;

        public PostObraValidator(IObraApplication obraApplication,
                                 IEditoraApplication editoraApplication,
                                 IGeneroApplication generoApplication,
                                 IAutorApplication autorApplication,
                                 IIdiomaApplication idiomaApplication)
        {
            this.obraApplication = obraApplication;
            this.editoraApplication = editoraApplication;
            this.generoApplication = generoApplication;
            this.autorApplication = autorApplication;
            this.idiomaApplication = idiomaApplication;

            RuleFor(x => x.Nome)
                .NotEmpty()
                .WithMessage("O campo {PropertyName} não pode ser nulo ou vazio.")
                .Must((dto, nome) => !obraApplication.ExisteNomePostDto(dto))
                .When(x => !string.IsNullOrEmpty(x.Nome))
                .WithMessage("Já existe uma obra cadastrada com o nome informado.");

            RuleFor(x => x.EditoraId)
                .NotEmpty()
                .WithMessage("O campo {PropertyName} não pode ser nulo ou vazio.")
                .Must(id => editoraApplication.ExisteId(id))
                .WithMessage("Nenhuma editora foi encontrada com o id informado.");

            When(x => x.Volumes != null && x.Volumes.Any(), () =>
            {
                RuleFor(x => x)
                    .Must(NaoExistemNomesDuplicadosEmVolumes)
                    .WithMessage("Não é possível inserir volumes com nomes idênticos na mesma obra.");
            });

            When(x => x.Idiomas != null && x.Idiomas.Any(), () =>
            {
                RuleFor(x => x)
                    .Must(NaoExisteIdiomaIdDuplicados)
                    .WithMessage("Não é possível inserir idiomas repetidos na mesma obra.");

                RuleForEach(x => x.Idiomas)
                    .SetValidator(new ReferenciaIdiomaValidator(idiomaApplication));
            });
        }

        private bool NaoExistemNomesDuplicadosEmVolumes(PostObraDto postObraDto)
        {
            return postObraDto.Volumes?.GroupBy(x => x.Nome).All(g => g.Count() == 1) ?? true;
        }

        private bool NaoExisteIdiomaIdDuplicados(PostObraDto postObraDto)
        {
            return postObraDto.Idiomas?.GroupBy(x => x.Id).All(g => g.Count() == 1) ?? true;
        }
    }
}