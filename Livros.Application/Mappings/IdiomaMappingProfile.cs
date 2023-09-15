using AutoMapper;
using Livros.Application.Dtos.Idioma;
using Livros.Domain.Entities;

namespace Livros.Application.Mappings
{
    public class IdiomaMappingProfile : Profile
    {
        public IdiomaMappingProfile()
        {
            Map();
        }

        private void Map()
        {
            CreateMap<PostIdiomaDto, Idioma>().ReverseMap();
            CreateMap<PutIdiomaDto, Idioma>().ReverseMap();
            CreateMap<Idioma, ViewIdiomaDto>().ReverseMap();
        }
    }
}