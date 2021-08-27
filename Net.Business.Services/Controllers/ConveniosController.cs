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
    [ApiExplorerSettings(GroupName = "ApiCliente")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class ConveniosController : ControllerBase
    {
        private readonly IRepositoryWrapper _repository;
        public ConveniosController(IRepositoryWrapper repository)
        {
            this._repository = repository;
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetConveniosPorFiltros([FromQuery] string codalmacen, string tipomovimiento, string codtipocliente, string codcliente, string codpaciente, string codaseguradora, string codcia, string codproducto)
        {

            var objectGetAll = await _repository.Convenios.GetConveniosPorFiltros(codalmacen, tipomovimiento, codtipocliente, codcliente, codpaciente, codaseguradora, codcia, codproducto);

            if (objectGetAll.ResultadoCodigo == -1)
            {
                return BadRequest(objectGetAll);
            }

            return Ok(objectGetAll.data);
        }
    }
}
