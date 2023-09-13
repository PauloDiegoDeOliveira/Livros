using AutoMapper;
using Livros.Application.Dtos.Volume;
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
            CreateMap<Volume, ViewVolumeDto>().ReverseMap();
        }
    }
}