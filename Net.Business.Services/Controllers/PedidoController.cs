using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Net.Business.DTO;
using Net.Data;

namespace Net.Business.Services.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ApiExplorerSettings(GroupName = "ApiPedido")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class PedidoController : ControllerBase
    {
        private readonly IRepositoryWrapper _repository;
        public PedidoController(IRepositoryWrapper repository)
        {
            this._repository = repository;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetListaPedidosSeguimientoPorFiltro([FromQuery] DateTime fechainicio, DateTime fechaFin, string ccosto, int opcion)
        {

            var objectGetAll = await _repository.Pedido.GetListaPedidosSeguimientoPorFiltro(fechainicio, fechaFin, ccosto, opcion);

            if (objectGetAll.ResultadoCodigo == -1)
            {
                return BadRequest(objectGetAll);
            }

            return Ok(objectGetAll.dataList);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetListPedidosPorAtencion([FromQuery] string codatencion, string codtercero)
        {

            var objectGetAll = await _repository.Pedido.GetListPedidosPorAtencion(codatencion, codtercero);

            if (objectGetAll.ResultadoCodigo == -1)
            {
                return BadRequest(objectGetAll);
            }

            var obj = new DtoPedidoPorAtencionListarResponse().RetornarListaPedidoPorAtencion(objectGetAll.dataList);

            return Ok(obj.ListaPedidoPorAtencion);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetListPedidoDetallePorPedido([FromQuery] string codpedido)
        {

            var objectGetAll = await _repository.Pedido.GetListPedidoDetallePorPedido(codpedido);

            if (objectGetAll.ResultadoCodigo == -1)
            {
                return BadRequest(objectGetAll);
            }

            return Ok(objectGetAll.dataList);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetListPedidosPorFiltro([FromQuery] DateTime fechainicio, DateTime fechafin, string codtipopedido, string codpedido)
        {

            var objectGetAll = await _repository.Pedido.GetListPedidosPorFiltro(fechainicio, fechafin, codtipopedido, codpedido);

            if (objectGetAll.ResultadoCodigo == -1)
            {
                return BadRequest(objectGetAll);
            }

            var obj = new DtoPedidoListarResponse().RetornarListaPedido(objectGetAll.dataList);


            return Ok(obj.ListaPedido);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetDatosPedidoPorPedido([FromQuery] string codpedido)
        {

            var objectGetAll = await _repository.Pedido.GetDatosPedidoPorPedido(codpedido);

            if (objectGetAll.ResultadoCodigo == -1)
            {
                return BadRequest(objectGetAll);
            }

            return Ok(objectGetAll.dataList);
        }
    }
}
