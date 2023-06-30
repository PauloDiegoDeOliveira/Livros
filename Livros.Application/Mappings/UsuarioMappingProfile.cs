using AutoMapper;
using Livros.Application.Dtos.Usuario;
using Livros.Domain.Entities;

namespace Livros.Application.Mappings
{
    public class UsuarioMappingProfile : Profile
    {
        public UsuarioMappingProfile()
        {
            Map();
        }

        private void Map()
        {
            CreateMap<PostCadastroUsuarioDto, Usuario>().ReverseMap();
            CreateMap<PutCadastroUsuarioDto, Usuario>().ReverseMap();
            CreateMap<Usuario, ViewUsuarioDto>().ReverseMap();
        }
    }
}