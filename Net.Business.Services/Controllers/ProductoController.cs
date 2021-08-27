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
    public class ProductoController : ControllerBase
    {
        private readonly IRepositoryWrapper _repository;
        public ProductoController(IRepositoryWrapper repository)
        {
            this._repository = repository;
        }

        /// <summary>
        /// Lista por filtro para ventas
        /// </summary>
        /// <param name="codalmacen"></param>
        /// <param name="codigo"></param>
        /// <param name="nombre"></param>
        /// <param name="codaseguradora"></param>
        /// <param name="codcia"></param>
        /// <returns></returns>
        //[HttpGet]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //public async Task<IActionResult> GetListProductoPorFiltro([FromQuery] string codalmacen, string codigo, string nombre, string codaseguradora, string codcia)
        //{

        //    var objectGetAll = await _repository.Producto.GetListProductoPorFiltro(codalmacen, codigo, nombre, codaseguradora, codcia);

        //    if (objectGetAll.ResultadoCodigo == -1)
        //    {
        //        return BadRequest(objectGetAll);
        //    }

        //    return Ok(objectGetAll.dataList);
        //}

        /// <summary>
        /// Lista Generico por codigo
        /// </summary>
        /// <param name="codigo"></param>
        /// <returns></returns>
        //[HttpGet]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //public async Task<IActionResult> GetListProductoGenericoPorCodigo([FromQuery] string codigo)
        //{

        //    var objectGetAll = await _repository.Producto.GetListProductoGenericoPorCodigo(codigo);

        //    if (objectGetAll.ResultadoCodigo == -1)
        //    {
        //        return BadRequest(objectGetAll);
        //    }

        //    return Ok(objectGetAll.dataList);
        //}

        
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetListProductoAlternativoPorCodigo([FromQuery] string codproducto, string codaseguradora, string codcia)
        {

            var objectGetAll = await _repository.Producto.GetListProductoAlternativoPorCodigo(codproducto, codaseguradora, codcia);

            if (objectGetAll.ResultadoCodigo == -1)
            {
                return BadRequest(objectGetAll);
            }

            return Ok(objectGetAll.dataList);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProductoPorCodigo([FromQuery] string codalmacen, string codproducto, string codaseguradora, string codcia, string tipomovimiento, string codtipocliente, string codcliente, string codpaciente, int tipoatencion)
        {

            var objectGetAll = await _repository.Producto.GetProductoPorCodigo(codalmacen, codproducto, codaseguradora, codcia, tipomovimiento, codtipocliente, codcliente, codpaciente, tipoatencion);

            if (objectGetAll.ResultadoCodigo == -1)
            {
                return BadRequest(objectGetAll);
            }

            return Ok(objectGetAll.data);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProductoyStockAlmacenesPorCodigo([FromQuery] string codproducto)
        {

            var objectGetAll = await _repository.Producto.GetProductoyStockAlmacenesPorCodigo(codproducto);

            if (objectGetAll.ResultadoCodigo == -1)
            {
                return BadRequest(objectGetAll);
            }

            return Ok(objectGetAll.data);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetListDetalleProductoPorPedido([FromQuery] string codpedido, string codalmacen, string codaseguradora, string codcia, string tipomovimiento, string codtipocliente, string codcliente, string codpaciente, int tipoatencion)
        {

            var objectGetAll = await _repository.Producto.GetListDetalleProductoPorPedido(codpedido, codalmacen, codaseguradora, codcia, tipomovimiento, codtipocliente, codcliente, codpaciente, tipoatencion);

            if (objectGetAll.ResultadoCodigo == -1)
            {
                return BadRequest(objectGetAll);
            }

            return Ok(objectGetAll.dataList);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetListDetalleProductoPorReceta([FromQuery] int idereceta, string codalmacen, string codaseguradora, string codcia, string tipomovimiento, string codtipocliente, string codcliente, string codpaciente, int tipoatencion)
        {

            var objectGetAll = await _repository.Producto.GetListDetalleProductoPorReceta(idereceta, codalmacen, codaseguradora, codcia, tipomovimiento, codtipocliente, codcliente, codpaciente, tipoatencion);

            if (objectGetAll.ResultadoCodigo == -1)
            {
                return BadRequest(objectGetAll);
            }

            return Ok(objectGetAll.dataList);
        }
    }
}
