﻿using Livros.Domain.Enums;

namespace Livros.Application.Dtos.Autor
{
    public class ViewAutorDto
    {
        public Guid Id { get; set; }
        public string UsuarioId { get; set; }

        public Guid ObraId { get; set; }
        public string Nome { get; set; }
        public EStatus Status { get; set; }
    }
}