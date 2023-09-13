using Microsoft.AspNetCore.Mvc;

namespace Livros.API.V1.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [ApiVersion("1.0")]
    [Route("/v{version:apiVersion}/versao")]
    [ApiController]
    public class VersaoController : ControllerBase
    {
        /// <summary>
        /// Informa a versão da API.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string Valor()
        {
            return "Esta é a versão V1.";
        }
    }
}