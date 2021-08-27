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
    [ApiExplorerSettings(GroupName = "ApiProducto")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class StockController : ControllerBase
    {
        private readonly IRepositoryWrapper _repository;
        public StockController(IRepositoryWrapper repository)
        {
            this._repository = repository;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetListStockPorFiltro([FromQuery] string codalmacen, string nombre, string codproducto, bool constock)
        {

            var objectGetAll = await _repository.Stock.GetListStockPorFiltro(codalmacen, nombre, codproducto, constock);

            if (objectGetAll.ResultadoCodigo == -1)
            {
                return BadRequest(objectGetAll);
            }

            return Ok(objectGetAll.dataList);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetListStockPorProductoAlmacen([FromQuery] string codalmacen, string codproducto)
        {

            var objectGetAll = await _repository.Stock.GetListStockPorProductoAlmacen(codalmacen, codproducto);

            if (objectGetAll.ResultadoCodigo == -1)
            {
                return BadRequest(objectGetAll);
            }

            return Ok(objectGetAll.dataList);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetListProductoGenericoPorCodigo([FromQuery] string codalmacen, string codprodci, bool constock)
        {

            var objectGetAll = await _repository.Stock.GetListProductoGenericoPorCodigo(codalmacen, codprodci, constock);

            if (objectGetAll.ResultadoCodigo == -1)
            {
                return BadRequest(objectGetAll);
            }

            return Ok(objectGetAll.dataList);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetListProductoGenericoPorDCI([FromQuery] string codalmacen, string coddci, bool constock)
        {

            var objectGetAll = await _repository.Stock.GetListProductoGenericoPorDCI(codalmacen, coddci, constock);

            if (objectGetAll.ResultadoCodigo == -1)
            {
                return BadRequest(objectGetAll);
            }

            return Ok(objectGetAll.dataList);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetListStockLotePorFiltro([FromQuery] string codalmacen, string codproducto, bool constock)
        {

            var objectGetAll = await _repository.Stock.GetListStockLotePorFiltro(codalmacen, codproducto, constock);

            if (objectGetAll.ResultadoCodigo == -1)
            {
                return BadRequest(objectGetAll);
            }

            return Ok(objectGetAll.dataList);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetListStockUbicacionPorFiltro([FromQuery] string codalmacen, string codproducto, bool constock)
        {

            var objectGetAll = await _repository.Stock.GetListStockUbicacionPorFiltro(codalmacen, codproducto, constock);

            if (objectGetAll.ResultadoCodigo == -1)
            {
                return BadRequest(objectGetAll);
            }

            return Ok(objectGetAll.dataList);
        }
    }
}
