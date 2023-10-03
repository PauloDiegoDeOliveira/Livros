﻿using AutoMapper;
using Livros.Application.Dtos.Genero;
using Livros.Domain.Entities;

namespace Livros.Application.Mappings
{
    public class GeneroMappingProfile : Profile
    {
        public GeneroMappingProfile()
        {
            Map();
        }

        private void Map()
        {
            CreateMap<PostGeneroDto, Genero>().ReverseMap();
            CreateMap<PutGeneroDto, Genero>().ReverseMap();

            CreateMap<Genero, ViewGeneroDto>().ForMember(viewObraDto => viewObraDto.QuantidadeObras,
                     opt => opt.MapFrom(editora => editora.Obras.Count()));
        }
    }
}