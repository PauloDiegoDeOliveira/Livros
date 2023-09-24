using FluentValidation;
using Livros.Application.Dtos.Autor;
using Livros.Application.Interfaces;

namespace Livros.Application.Validators.Autor
{
    public class PutAutorValidator : AbstractValidator<PutAutorDto>
    {
        private readonly IAutorApplication autorApplication;

        public PutAutorValidator(IAutorApplication autorApplication)
        {
            this.autorApplication = autorApplication;

            RuleFor(x => x.Id)
                .NotNull()
                .WithMessage("O campo {PropertyName} não pode ser nulo.")

                .NotEmpty()
                .WithMessage("O campo {PropertyName} não pode ser vazio.")

                .Must((dto, cancelar) =>
                {
                    return ExisteId(dto.Id);
                }).WithMessage("Nenhum autor foi encontrado com o id informado.");

            RuleFor(x => x.Nome)
                .NotNull()
                .WithMessage("O campo {PropertyName} não pode ser nulo.")

                .NotEmpty()
                .WithMessage("O campo {PropertyName} não pode ser vazio.");

            When(x => x.Nome != null, () =>
            {
                RuleFor(x => x)
                    .Must((dto, cancellation) =>
                    {
                        return !ExisteNomePutDto(dto);
                    }).WithMessage("Já existe um autor cadastrado com o nome informado.");
            });

            RuleFor(x => x.Status)
                .NotNull()
                .WithMessage("O campo {PropertyName} não pode ser nulo.")

                .NotEmpty()
                .WithMessage("O campo {PropertyName} não pode ser vazio.");
        }

        private bool ExisteId(Guid id)
        {
            return autorApplication.ExisteId(id);
        }

        private bool ExisteNomePutDto(PutAutorDto putAutorDto)
        {
            return autorApplication.ExisteNomePutDto(putAutorDto);
        }
    }
}