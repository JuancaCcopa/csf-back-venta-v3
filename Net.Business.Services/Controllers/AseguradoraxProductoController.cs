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
    public class AseguradoraxProductoController : ControllerBase
    {
        private readonly IRepositoryWrapper _repository;
        public AseguradoraxProductoController(IRepositoryWrapper repository)
        {
            this._repository = repository;
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetListAseguradoraxProductoPorFiltros([FromQuery] string codaseguradora, string codproducto, int tipoatencion)
        {

            var objectGetAll = await _repository.AseguradoraxProducto.GetListAseguradoraxProductoPorFiltros(codaseguradora, codproducto, tipoatencion);

            if (objectGetAll.ResultadoCodigo == -1)
            {
                return BadRequest(objectGetAll);
            }

            return Ok(objectGetAll.dataList);
        }
    }
}
