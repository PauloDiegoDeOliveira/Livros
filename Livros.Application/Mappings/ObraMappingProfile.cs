using AutoMapper;
using Livros.Application.Dtos.Obra;
using Livros.Application.Utilities.Extensions;
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

            CreateMap<Obra, ViewObraDetalhesDto>().ForMember(viewObraDto => viewObraDto.PrecoTotal,
                                                             opt => opt.MapFrom(obra => obra.Volumes.Sum(volume => volume.Preco).MoedaBrasileira()))
                                                  .ForMember(viewObraDto => viewObraDto.AvaliacaoTotal,
                                                             opt => opt.MapFrom(obra => obra.Volumes.Sum(volume => volume.Avaliacao)))
                                                  .ForMember(viewObraDto => viewObraDto.PaginaTotal,
                                                             opt => opt.MapFrom(obra => obra.Volumes.Sum(volume => volume.Paginas)));

            CreateMap<Obra, ViewObraDto>()
                  .ForMember(viewObraDto => viewObraDto.PrecoTotal,
                             opt => opt.MapFrom(obra => obra.Volumes.Sum(volume => volume.Preco).MoedaBrasileira()))
                  .ForMember(viewObraDto => viewObraDto.AvaliacaoTotal,
                             opt => opt.MapFrom(obra => obra.Volumes.Sum(volume => volume.Avaliacao)))
                  .ForMember(viewObraDto => viewObraDto.PaginaTotal,
                             opt => opt.MapFrom(obra => obra.Volumes.Sum(volume => volume.Paginas)));
        }
    }
}