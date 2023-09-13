using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Livros.Application.Dtos.Volume
{
    public class PostVolumeDto
    {
        /// <summary>
        /// ObraId
        /// </summary>
        /// <example>EBBD2417-8FF7-482C-9768-08DAD14324AE</example>
        [JsonIgnore]
        public Guid ObraId { get; set; }

        /// <summary>
        /// Título
        /// </summary>
        /// <example>Volume</example>
        [Display(Name = "título")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(150, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 2)]
        public string Titulo { get; set; }

        /// <summary>
        /// Número
        /// </summary>
        /// <example>1</example>
        [Display(Name = "número")]
        public int Numero { get; set; }

        /// <summary>
        /// Páginas
        /// </summary>
        /// <example>500</example>
        [Display(Name = "páginas")]
        public int Paginas { get; set; }

        /// <summary>
        /// Anotação
        /// </summary>
        /// <example>1984, obra-prima de George Orwell, é uma distopia sombria e poderosa que explora um futuro totalitário. A história segue Winston Smith, um funcionário público do partido que controla Oceânia, uma das três superpotências do mundo, com o olho sempre vigiante do Grande Irmão. Winston se encontra desafiando o sistema, em meio à repressão e lavagem cerebral, se arriscando em um ato de rebelião - amar. Enquanto a polícia do pensamento se esforça para manter o status quo, Winston luta pela liberdade individual. Uma leitura aterrorizante e perturbadora sobre autoritarismo, vigilância e a erosão da verdade.</example>
        [Display(Name = "anotação")]
        public string Anotacao { get; set; }

        /// <summary>
        /// Preço
        /// </summary>
        /// <example>100</example>
        [Display(Name = "preço")]
        public decimal Preco { get; set; }

        /// <summary>
        /// Avaliação
        /// </summary>
        /// <example>5</example>
        [Display(Name = "avaliação")]
        public int Avaliacao { get; set; }

        /// <summary>
        /// Lido
        /// </summary>
        public bool Lido { get; set; }

        /// <summary>
        /// Data de leitura
        /// </summary>
        /// <example>1984-05-20</example>
        [Display(Name = "data de leitura")]
        [DataType(DataType.DateTime, ErrorMessage = "O campo {0} está em formato inválido.")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DataLeitura { get; set; }

        /// <summary>
        /// Data de compra
        /// </summary>
        /// <example>1984-05-20</example>
        [Display(Name = "data de compra")]
        [DataType(DataType.DateTime, ErrorMessage = "O campo {0} está em formato inválido.")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DataCompra { get; set; }
    }
}