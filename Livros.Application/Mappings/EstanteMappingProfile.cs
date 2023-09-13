using AutoMapper;
using Livros.Application.Dtos.Estante;
using Livros.Application.Dtos.Lista;
using Livros.Domain.Entities;

namespace Livros.Application.Mappings
{
    public class EstanteMappingProfile : Profile
    {
        public EstanteMappingProfile()
        {
            Map();
        }

        private void Map()
        {
            CreateMap<PostEstanteDto, Estante>().ReverseMap();
            CreateMap<PutEstanteDto, Estante>().ReverseMap();
            CreateMap<Estante, ViewEstanteDto>().ReverseMap();
        }
    }
}