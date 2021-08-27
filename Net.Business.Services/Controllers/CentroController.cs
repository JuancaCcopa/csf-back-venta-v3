using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Net.Data;

namespace Net.Business.Services.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ApiExplorerSettings(GroupName = "ApiCentro")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class CentroController : ControllerBase
    {
        private readonly IRepositoryWrapper _repository;
        public CentroController(IRepositoryWrapper repository)
        {
            this._repository = repository;
        }

        /// <summary>
        /// Lista los almacenes que tienen la palabra filtrada
        /// </summary>
        /// <param name="nombre">palabra filtrada</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetListCentroContains([FromQuery] string nombre)
        {

            var objectGetAll = await _repository.CentroCosto.GetListCentroContains(nombre);

            if (objectGetAll.ResultadoCodigo == -1)
            {
                return BadRequest(objectGetAll);
            }

            return Ok(objectGetAll.dataList);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCentroPorCodigo([FromQuery] string codcentro)
        {

            var objectGetAll = await _repository.CentroCosto.GetCentroPorCodigo(codcentro);

            if (objectGetAll.ResultadoCodigo == -1)
            {
                return BadRequest(objectGetAll);
            }

            return Ok(objectGetAll.data);
        }
    }
}
