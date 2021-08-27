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
    [ApiExplorerSettings(GroupName = "ApiApu")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class ApuController : ControllerBase
    {
        private readonly IRepositoryWrapper _repository;
        public ApuController(IRepositoryWrapper repository)
        {
            this._repository = repository;
        }

        /// <summary>
        /// Listara los pedidos por rango de fechas
        /// </summary>
        /// <param name="fechainicio"> Fecha de Inicio</param>
        /// <param name="fechafin">Fecha Fin</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetListPedidosPorRangoFechas([FromQuery] DateTime fechainicio, DateTime fechafin)
        {

            var objectGetAll = await _repository.Pedido.GetListPedidosPorFiltro(fechainicio, fechafin, string.Empty, string.Empty);

            if (objectGetAll.ResultadoCodigo == -1)
            {
                return BadRequest(objectGetAll);
            }

            var obj = new DtoPedidoApuListarResponse().RetornarListaPedido(objectGetAll.dataList).ListaPedido;

            return Ok(obj);
        }

        /// <summary>
        /// Listara el detalle del pedido
        /// </summary>
        /// <param name="codpedido">Codigo de Pedido</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetListDetallePedidoPorPedido([FromQuery] string codpedido)
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
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetListPickingPorPedido([FromQuery] string codpedido)
        {

            var objectGetAll = await _repository.Picking.GetListPickingPorPedido(codpedido);

            if (objectGetAll.ResultadoCodigo == -1)
            {
                return BadRequest(objectGetAll);
            }

            return Ok(objectGetAll.dataList);
        }

        /// <summary>
        /// Listara las recetas por rango de fechas
        /// </summary>
        /// <param name="fechainicio">Fecha Inicio</param>
        /// <param name="fechafin">Fecha Fin</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetListRecetasPorRangoFechas([FromQuery] DateTime fechainicio, DateTime fechafin)
        {

            var objectGetAll = await _repository.Receta.GetListRecetasPorFiltro(fechainicio, fechafin, string.Empty, 0, string.Empty, string.Empty);

            if (objectGetAll.ResultadoCodigo == -1)
            {
                return BadRequest(objectGetAll);
            }

            var obj = new DtoRecetaApuListarResponse().RetornarListaReceta(objectGetAll.dataList);

            return Ok(obj.ListaReceta);
        }

        /// <summary>
        /// Listara el detalle de la receta
        /// </summary>
        /// <param name="id">Id Receta</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetListDetalleRecetaPorReceta([FromQuery] int id)
        {

            var objectGetAll = await _repository.Receta.GetListRecetaDetallePorReceta(id);

            if (objectGetAll.ResultadoCodigo == -1)
            {
                return BadRequest(objectGetAll);
            }

            return Ok(objectGetAll.dataList);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetListPickingPorReceta([FromQuery] int id)
        {

            var objectGetAll = await _repository.Picking.GetListPickingPorReceta(id);

            if (objectGetAll.ResultadoCodigo == -1)
            {
                return BadRequest(objectGetAll);
            }

            return Ok(objectGetAll.dataList);
        }

        [HttpGet(Name = "GetPickingPorId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetPickingPorId([FromQuery] int id)
        {

            var objectGetAll = await _repository.Picking.GetPickingPorId(id);

            if (objectGetAll.ResultadoCodigo == -1)
            {
                return BadRequest(objectGetAll);
            }

            return Ok(objectGetAll.dataList);
        }

        /// <summary>
        /// Registrar Picking
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RegistrarPicking([FromBody] DtoPickingRegistrar value)
        {
            try
            {
                if (value == null)
                {
                    return BadRequest("Master object is null");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid model object");
                }

                var response = await _repository.Picking.Registrar(value.RetornaPicking());

                if (response.ResultadoCodigo == -1)
                {
                    return BadRequest(response);
                }

                return CreatedAtRoute("GetPickingPorId", new { id = response.IdRegistro }, response.data);
            }
            catch (Exception ex)
            {
                return BadRequest($"Hubo un error en la solicitud : { ex.Message.ToString() }");
            }

        }

        /// <summary>
        /// Actualizar Estado del Picking Pedido
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ModificarEstadoPedido([FromBody] DtoPickingModificarEstadoPedido value)
        {
            try
            {
                if (value == null)
                {
                    return BadRequest(ModelState);
                }

                var response = await _repository.Picking.ModificarEstadoPedido(value.RetornaPicking());

                if (response.ResultadoCodigo == -1)
                {
                    return BadRequest(response);
                }
                else
                {
                    return NoContent();
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Hubo un error en la solicitud : { ex.Message.ToString() }");
            }

        }

        /// <summary>
        /// Actualizar Estado del Picking receta
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ModificarEstadoReceta([FromBody] DtoPickingModificarEstadoReceta value)
        {
            try
            {
                if (value == null)
                {
                    return BadRequest(ModelState);
                }

                var response = await _repository.Picking.ModificarEstadoReceta(value.RetornaPicking());

                if (response.ResultadoCodigo == -1)
                {
                    return BadRequest(response);
                }
                else
                {
                    return NoContent();
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Hubo un error en la solicitud : { ex.Message.ToString() }");
            }

        }

        /// <summary>
        /// Eliminar registro logicos
        /// </summary>
        /// <param name="value">Body para eliminar</param>
        /// <returns></returns>
        [HttpPatch]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> EliminarPicking([FromBody] DtoPickingEliminar value)
        {
            try
            {
                if (value == null)
                {
                    return BadRequest(ModelState);
                }

                var response = await _repository.Picking.Eliminar(value.RetornaPicking());

                if (response.ResultadoCodigo == -1)
                {
                    return BadRequest(response);
                }
                else
                {
                    return NoContent();
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Hubo un error en la solicitud : { ex.Message.ToString() }");
            }
        }

        /// <summary>
        /// Listado de Consolidados
        /// </summary>
        /// <param name="fechainicio">Fecha Inicio</param>
        /// <param name="fechafin">Fecha Fin</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetListConsolidadoPorFiltro([FromQuery] DateTime fechainicio, DateTime fechafin)
        {

            var objectGetAll = await _repository.Consolidado.GetListConsolidadoPorFiltro(fechainicio, fechafin);

            if (objectGetAll.ResultadoCodigo == -1)
            {
                return BadRequest(objectGetAll);
            }

            return Ok(objectGetAll.dataList);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetListConsolidadoPicking([FromQuery] string codpedido, string codproducto)
        {

            var objectGetAll = await _repository.Consolidado.GetListConsolidadoPicking(codpedido, codproducto);

            if (objectGetAll.ResultadoCodigo == -1)
            {
                return BadRequest(objectGetAll);
            }

            return Ok(objectGetAll.dataList);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetListConsolidadoPickingPorId([FromQuery] int id)
        {

            var objectGetAll = await _repository.Consolidado.GetListConsolidadoPickingPorId(id);

            if (objectGetAll.ResultadoCodigo == -1)
            {
                return BadRequest(objectGetAll);
            }

            return Ok(objectGetAll.dataList);
        }

        /// <summary>
        /// Listara el detalle de un consolidado especifico
        /// </summary>
        /// <param name="idconsolidado">Id Consolidado</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetListDetalleConsolidado([FromQuery] int idconsolidado)
        {

            var objectGetAll = await _repository.Consolidado.GetListDetalleConsolidado(idconsolidado);

            if (objectGetAll.ResultadoCodigo == -1)
            {
                return BadRequest(objectGetAll);
            }

            return Ok(objectGetAll.dataList);
        }

        [HttpGet(Name = "GetConsolidadoPedidoPickingPorId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetConsolidadoPedidoPickingPorId([FromQuery] int id)
        {

            var objectGetAll = await _repository.Consolidado.GetConsolidadoPedidoPickingPorId(id);

            if (objectGetAll.ResultadoCodigo == -1)
            {
                return BadRequest(objectGetAll);
            }

            return Ok(objectGetAll.dataList);
        }

        /// <summary>
        /// Registrar Consolidado - Picking
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RegistrarConsolidadoPicking([FromBody] DtoConsolidadoPedidoPickingRegistrar value)
        {
            try
            {
                if (value == null)
                {
                    return BadRequest("Master object is null");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid model object");
                }

                var response = await _repository.Consolidado.RegistrarConsolidadoPicking(value.RetornaConsolidadoPedidoPicking());

                if (response.ResultadoCodigo == -1)
                {
                    return BadRequest(response);
                }

                return CreatedAtRoute("GetConsolidadoPedidoPickingPorId", new { id = response.IdRegistro }, response.data);
            }
            catch (Exception ex)
            {
                return BadRequest($"Hubo un error en la solicitud : { ex.Message.ToString() }");
            }

        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ModificarConsolidadoPicking([FromBody] DtoConsolidadoPedidoPickingModificar value)
        {
            try
            {
                if (value == null)
                {
                    return BadRequest(ModelState);
                }

                var response = await _repository.Consolidado.ModificarConsolidadoPicking(value.RetornaConsolidadoPedidoPicking());

                if (response.ResultadoCodigo == -1)
                {
                    return BadRequest(response);
                }
                else
                {
                    return NoContent();
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Hubo un error en la solicitud : { ex.Message.ToString() }");
            }

        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ModificarConsolidadoPickingEstado([FromBody] DtoConsolidadoPedidoPickingModificarEstado value)
        {
            try
            {
                if (value == null)
                {
                    return BadRequest(ModelState);
                }

                var response = await _repository.Consolidado.ModificarEstadoPedido(value.RetornaConsolidadoPedidoPicking());

                if (response.ResultadoCodigo == -1)
                {
                    return BadRequest(response);
                }
                else
                {
                    return NoContent();
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Hubo un error en la solicitud : { ex.Message.ToString() }");
            }

        }

        [HttpPatch]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> EliminarConsolidadoPicking([FromBody] DtoConsolidadoPedidoPickingEliminar value)
        {
            try
            {
                if (value == null)
                {
                    return BadRequest(ModelState);
                }

                var response = await _repository.Consolidado.EliminarConsolidadoPicking(value.RetornaConsolidadoPedidoPicking());

                if (response.ResultadoCodigo == -1)
                {
                    return BadRequest(response);
                }
                else
                {
                    return NoContent();
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Hubo un error en la solicitud : { ex.Message.ToString() }");
            }
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ModificarVentaCabeceraEnvioPiso([FromBody] DtoVentaCabeceraEnvioPisoModificar value)
        {
            try
            {
                if (value == null)
                {
                    return BadRequest(ModelState);
                }

                var response = await _repository.Venta.ModificarVentaCabeceraEnvioPiso(value.RetornaVentasCabeceraResponse());

                if (response.ResultadoCodigo == -1)
                {
                    return BadRequest(response);
                }
                else
                {
                    return NoContent();
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Hubo un error en la solicitud : { ex.Message.ToString() }");
            }

        }
    }
}
