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
    [ApiExplorerSettings(GroupName = "ApiVenta")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class VentaController : ControllerBase
    {
        private readonly IRepositoryWrapper _repository;
        public VentaController(IRepositoryWrapper repository)
        {
            this._repository = repository;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAll([FromQuery] string codcomprobante, string codventa, DateTime fecinicio, DateTime fecfin)
        {

            var objectGetAll = await _repository.Venta.GetAll(codcomprobante, codventa, fecinicio, fecfin);

            if (objectGetAll.ResultadoCodigo == -1)
            {
                return BadRequest(objectGetAll);
            }

            var obj = new DtoVentaCabeceraListarResponse().RetornarListaVentaCabecera(objectGetAll.dataList);

            return Ok(obj.ListaVentaCabecera);
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllSinStock([FromQuery] string codcomprobante, string codventa, DateTime fecinicio, DateTime fecfin)
        {

            var objectGetAll = await _repository.Venta.GetAllSinStock(codcomprobante, codventa, fecinicio, fecfin);

            if (objectGetAll.ResultadoCodigo == -1)
            {
                return BadRequest(objectGetAll);
            }

            var obj = new DtoVentaCabeceraListarResponse().RetornarListaVentaCabecera(objectGetAll.dataList);

            return Ok(obj.ListaVentaCabecera);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="codventa"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetVentaPorCodVenta([FromQuery] string codventa)
        {

            var objectGetAll = await _repository.Venta.GetVentaPorCodVenta(codventa);

            if (objectGetAll.ResultadoCodigo == -1)
            {
                return BadRequest(objectGetAll);
            }

            return Ok(objectGetAll.data);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetVentaCabeceraPendientePorFiltro([FromQuery] DateTime fecha)
        {

            var objectGetAll = await _repository.Venta.GetVentaCabeceraPendientePorFiltro(fecha);

            if (objectGetAll.ResultadoCodigo == -1)
            {
                return BadRequest(objectGetAll);
            }

            var obj = new DtoVentaCabeceraListarResponse().RetornarListaVentaCabecera(objectGetAll.dataList);

            return Ok(obj.ListaVentaCabecera);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetVentaChequea1MesPorFiltro([FromQuery] string codpaciente, int cuantosmesesantes)
        {

            var objectGetAll = await _repository.Venta.GetVentaChequea1MesPorFiltro(codpaciente, cuantosmesesantes);

            if (objectGetAll.ResultadoCodigo == -1)
            {
                return BadRequest(objectGetAll);
            }

            var obj = new DtoVentaDetalle1MesListarResponse().RetornarVentaDetalle1MesListarResponse(objectGetAll.dataList);

            return Ok(obj.ListaVentaDetalle);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetGastoCubiertoPorFiltro([FromQuery] string codaseguradora, string codproducto, int tipoatencion)
        {

            var objectGetAll = await _repository.Venta.GetGastoCubiertoPorFiltro(codaseguradora, codproducto, tipoatencion);

            if (objectGetAll.ResultadoCodigo == -1)
            {
                return BadRequest(objectGetAll);
            }

            return Ok(objectGetAll.data);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetIPorFiltro([FromQuery] string codaseguradora, string codproducto, int tipoatencion)
        {

            var objectGetAll = await _repository.Venta.GetGastoCubiertoPorFiltro(codaseguradora, codproducto, tipoatencion);

            if (objectGetAll.ResultadoCodigo == -1)
            {
                return BadRequest(objectGetAll);
            }

            return Ok(objectGetAll.data);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ValidacionRegistraVentaCabecera([FromBody] DtoVentaCabeceraRegistrar value)
        {
            try
            {
                if (value == null)
                {
                    return BadRequest(ModelState);
                }

                var response = await _repository.Venta.ValidacionRegistraVentaCabecera(value.RetornaVentasCabecera());

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

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RegistrarVentaCabecera([FromBody] DtoVentaCabeceraRegistrar value)
        {
            try
            {
                if (value == null)
                {
                    return BadRequest(ModelState);
                }

                var response = await _repository.Venta.RegistrarVentaCabecera(value.RetornaVentasCabecera(), false);

                if (response.ResultadoCodigo == -1)
                {
                    return BadRequest(response);
                }
                else
                {
                    return Ok(response.dataList);
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Hubo un error en la solicitud : { ex.Message.ToString() }");
            }

        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RegistrarVentaDevolucion([FromBody] DtoVentaDevolucionRegistrar value)
        {
            try
            {
                if (value == null)
                {
                    return BadRequest(ModelState);
                }

                var response = await _repository.Venta.RegistrarVentaDevolucion(value.RetornaModelo());

                if (response.ResultadoCodigo == -1)
                {
                    return BadRequest(response);
                }
                else
                {
                    return Ok(response.dataList);
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Hubo un error en la solicitud : { ex.Message.ToString() }");
            }

        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetVentasChequea1MesAntes([FromQuery] string codpaciente, int cuantosmesesantes)
        {

            var objectGetAll = await _repository.Venta.GetVentasChequea1MesAntes(codpaciente, cuantosmesesantes);

            if (objectGetAll.ResultadoCodigo == -1)
            {
                return BadRequest(objectGetAll);
            }

            var obj = new DtoVentaDetalle1MesListarResponse().RetornarVentaDetalle1MesListarResponse(objectGetAll.dataList);

            return Ok(obj.ListaVentaDetalle);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ValidacionAnularVenta([FromBody] DtoVentaAnular value)
        {
            try
            {
                if (value == null)
                {
                    return BadRequest(ModelState);
                }

                var response = await _repository.Venta.ValidacionAnularVenta(value.RetornaVentaAnular());

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

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RegistrarAnularVenta([FromBody] DtoVentaAnular value)
        {
            try
            {
                if (value == null)
                {
                    return BadRequest(ModelState);
                }

                var response = await _repository.Venta.RegistrarAnularVenta(value.RetornaVentaAnular());

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

        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GeneraVentaAutomatica()
        {
            try
            {

                var response = await _repository.Venta.GeneraVentaAutomatica(string.Empty);

                if (response.ResultadoCodigo == -1)
                {
                    return BadRequest(response);
                }
                else
                {
                    return Ok(response.dataList);
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Hubo un error en la solicitud : { ex.Message.ToString() }");
            }

        }

        [HttpGet("{codventa}", Name = "GenerarValeVentaPrint")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public async Task<FileContentResult> GenerarValeVentaPrint(string codventa)
        {
            var objectGetById = await _repository.Venta.GenerarValeVentaPrint(codventa);

            var pdf = File(objectGetById.data.GetBuffer(), "applicacion/pdf", codventa + ".pdf");

            return pdf;
        }

        //[HttpPost]
        //[AllowAnonymous]
        //[ProducesResponseType(StatusCodes.Status204NoContent)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //public async Task<IActionResult> GeneraVentaAutomatica()
        //{
        //    try
        //    {

        //        var response = await _repository.Venta.GeneraVentaAutomatica(string.Empty);

        //        if (response.ResultadoCodigo == -1)
        //        {
        //            return BadRequest(response);
        //        }
        //        else
        //        {
        //            return Ok(response.dataList);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest($"Hubo un error en la solicitud : { ex.Message.ToString() }");
        //    }

        //}

        [HttpGet("{codventa}", Name = "GenerarValeVentaLotePrint")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public async Task<FileContentResult> GenerarValeVentaLotePrint(string codventa)
        {
            var objectGetById = await _repository.Venta.GenerarValeVentaLotePrint(codventa);

            var pdf = File(objectGetById.data.GetBuffer(), "applicacion/pdf", codventa + ".pdf");

            return pdf;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateSinStockVenta([FromBody] DtoVentaUpdateSinStock value)
        {
            try
            {
                if (value == null)
                {
                    return BadRequest(ModelState);
                }

                var response = await _repository.Venta.UpdateSinStockVenta(value.RetornaVentaUpdateSinStock());

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
        public async Task<IActionResult> UpdateSeguimiento([FromBody] DtoSeguimientoRegistrar value)
        {
            try
            {
                if (value == null)
                {
                    return BadRequest(ModelState);
                }

                var response = await _repository.Seguimiento.SetUpdateSeguimiento(value.RetornaModelo());

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
