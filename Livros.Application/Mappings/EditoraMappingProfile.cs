using AutoMapper;
using Livros.Application.Dtos.Editora;
using Livros.Domain.Entities;

namespace Livros.Application.Mappings
{
    public class EditoraMappingProfile : Profile
    {
        public EditoraMappingProfile()
        {
            Map();
        }

        private void Map()
        {
            CreateMap<PostEditoraDto, Editora>().ReverseMap();
            CreateMap<PutEditoraDto, Editora>().ReverseMap();
            CreateMap<Editora, ViewEditoraDto>().ReverseMap();
        }
    }
}