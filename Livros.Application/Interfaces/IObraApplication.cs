using Livros.Application.Dtos.Base;
using Livros.Application.Dtos.Obra;
using Livros.Application.Dtos.Pagination;
using Livros.Application.Dtos.Volume;
using Livros.Application.Interfaces.Base;
using Livros.Domain.Entities;
using Livros.Domain.Pagination;

namespace Livros.Application.Interfaces
{
    public interface IObraApplication : IApplicationBase<Obra, ViewObraDto, PostObraDto, PutObraDto, PutStatusDto>
    {
        Task<ViewPagedListDto<Obra, ViewObraDto>> GetPaginationAsync(ParametersObra parametersObra);

        Task<ViewObraDto> PostAsync(PostObraDto postObraDto, string caminhoFisico, string caminhoAbsoluto, string splitRelativo, string base64string, string extensao);

        Task<ViewObraDto> PutAsync(PutObraDto putObraDto, string caminhoFisico, string caminhoAbsoluto, string splitRelativo, string base64string, string extensao);

        Task<ViewObraDetalhesDto> GetByIdDetalhesAsync(Guid obraId);

        bool ExisteId(Guid id);

        bool ExisteNomePostDto(PostObraDto postObraDto);

        bool ExisteNomePutDto(PutObraDto putObraDto);

        bool ExisteVolumeId(Guid id);

        bool ExisteNomeVolumePutDto(PutVolumeDto putVolumeDto);

        bool ExisteNumeroVolumePostDto(PostVolumeDto postVolumeDto);

        bool ExisteNumeroVolumePutDto(PutVolumeDto putVolumeDto);
    }
}