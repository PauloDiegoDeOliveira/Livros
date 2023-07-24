using AutoMapper;
using Livros.Application.Dtos.Obra;
using Livros.Domain.Entities;

namespace Livros.Application.Mappings
{
    public class ObraMappingProfile : Profile
    {
        public ObraMappingProfile()
        {
            Map();
        }

        private void Map()
        {
            CreateMap<PostObraDto, Obra>().ReverseMap();
            CreateMap<PutObraDto, Obra>().ReverseMap();
            CreateMap<Obra, ViewObraDto>().ReverseMap();
        }
    }
}