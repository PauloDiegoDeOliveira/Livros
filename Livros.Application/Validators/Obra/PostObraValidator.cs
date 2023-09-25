using FluentValidation;
using Livros.Application.Dtos.Obra;
using Livros.Application.Interfaces;
using Livros.Application.Validators.Idioma;
using Livros.Application.Validators.Volume;

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

            RuleFor(x => x.Titulo)
                .NotEmpty()
                .WithMessage("O campo {PropertyName} não pode ser nulo ou vazio.")
                .Must((dto, nome) => !obraApplication.ExisteNomePostDto(dto))
                .When(x => !string.IsNullOrEmpty(x.Titulo))
                .WithMessage("Já existe uma obra cadastrada com o nome informado.");

            RuleFor(x => x.EditoraId)
                .NotEmpty()
                .WithMessage("O campo {PropertyName} não pode ser nulo ou vazio.")
                .Must(id => editoraApplication.ExisteId(id))
                .WithMessage("Nenhuma editora foi encontrada com o id informado.");

            RuleFor(x => x.GeneroId)
                .NotEmpty()
                .WithMessage("O campo {PropertyName} não pode ser nulo ou vazio.")
                .Must(id => generoApplication.ExisteId(id))
                .WithMessage("Nenhum género foi encontrado com o id informado.");

            RuleFor(x => x.AutorId)
                .NotEmpty()
                .WithMessage("O campo {PropertyName} não pode ser nulo ou vazio.")
                .Must(id => autorApplication.ExisteId(id))
                .WithMessage("Nenhum autor foi encontrado com o id informado.");

            //When(x => x.Volumes != null && x.Volumes.Any(), () =>
            //{
            //    RuleForEach(x => x.Volumes)
            //        .SetValidator(new PostVolumeValidator(obraApplication));
            //});

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