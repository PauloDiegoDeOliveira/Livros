using AutoMapper;
using Livros.Application.Dtos.Volume;
using Livros.Application.Utilities.Extensions;
using Livros.Domain.Entities;

namespace Livros.Application.Mappings
{
    public class VolumeMappingProfile : Profile
    {
        public VolumeMappingProfile()
        {
            Map();
        }

        private void Map()
        {
            CreateMap<PostVolumeDto, Volume>().ReverseMap();
            CreateMap<PutVolumeDto, Volume>().ReverseMap();

            CreateMap<Volume, ViewVolumeDto>()
                .ForMember(viewObraDto => viewObraDto.Preco,
                           opt => opt.MapFrom(volume => volume.Preco.MoedaBrasileira()))
                .ForMember(viewObraDto => viewObraDto.DataLeitura,
                           opt => opt.MapFrom(volume => volume.DataLeitura.DataFormatoBrasileiro()))
                .ForMember(viewObraDto => viewObraDto.DataCompra,
                           opt => opt.MapFrom(volume => volume.DataCompra.DataFormatoBrasileiro()));
        }
    }
}