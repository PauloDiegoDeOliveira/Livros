using Livros.Application.Dtos.Volume;
using Livros.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Livros.Application.Dtos.Obra
{
    public class PostObraDto
    {
        //[Display(Name = "imagem")]
        //[Required(ErrorMessage = "O campo {0} é obrigatório.")]
        //public IFormFile ImagemUpload { get; set; }

        ///// <summary>
        ///// ImagemId
        ///// </summary>
        ///// <example>EBBD2417-8FF7-482C-9768-08DAD14324AE</example>
        //public Guid ImagemId { get; set; }

        /// <summary>
        /// IdiomaId
        /// </summary>
        /// <example>EBBD2417-8FF7-482C-9768-08DAD14324AE</example>
        public Guid IdiomaId { get; set; }

        /// <summary>
        /// Título
        /// </summary>
        /// <example>1984</example>
        [Display(Name = "título")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(150, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 2)]
        public string Titulo { get; set; }

        /// <summary>
        /// Anotação
        /// </summary>
        /// <example>1984, obra-prima de George Orwell, é uma distopia sombria e poderosa que explora um futuro totalitário. A história segue Winston Smith, um funcionário público do partido que controla Oceânia, uma das três superpotências do mundo, com o olho sempre vigiante do Grande Irmão. Winston se encontra desafiando o sistema, em meio à repressão e lavagem cerebral, se arriscando em um ato de rebelião - amar. Enquanto a polícia do pensamento se esforça para manter o status quo, Winston luta pela liberdade individual. Uma leitura aterrorizante e perturbadora sobre autoritarismo, vigilância e a erosão da verdade.</example>
        [Display(Name = "anotação")]
        public string Anotacao { get; set; }

        /// <summary>
        /// Avaliação total
        /// </summary>
        /// <example>5</example>
        [Display(Name = "avaliação total")]
        public int AvaliacaoTotal { get; set; }

        /// <summary>
        /// Preço total
        /// </summary>
        /// <example>20</example>
        [Display(Name = "preço total")]
        public decimal PrecoTotal { get; set; }

        /// <summary>
        /// Página total
        /// </summary>
        /// <example>500</example>
        [Display(Name = "página total")]
        public int PaginaTotal { get; set; }

        /// <summary>
        /// Concluído
        /// </summary>
        [Display(Name = "concluído")]
        public bool Concluido { get; set; }

        /// <summary>
        /// Volume único
        /// </summary>
        [Display(Name = "volume único")]
        public bool VolumeUnico { get; set; }

        /// <summary>
        /// Tipo
        /// </summary>
        [Display(Name = "tipo")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [EnumDataType(typeof(ETipo), ErrorMessage = "O campo {0} está em formato inválido.")]
        public ETipo Tipo { get; set; }

        /// <summary>
        /// Volumes
        /// </summary>
        public List<PostVolumeDto> Volumes { get; set; }
    }
}