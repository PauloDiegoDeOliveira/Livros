using AutoMapper;
using Livros.Application.Dtos.Autor;
using Livros.Domain.Entities;

namespace Livros.Application.Mappings
{
    public class AutorMappingProfile : Profile
    {
        public AutorMappingProfile()
        {
            Map();
        }

        private void Map()
        {
            CreateMap<PostAutorDto, Autor>().ReverseMap();
            CreateMap<PutAutorDto, Autor>().ReverseMap();
            CreateMap<Autor, ViewAutorDto>().ReverseMap();
        }
    }
}