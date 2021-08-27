using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Net.Business.DTO;
using Net.Business.DTO.CuadreCaje;
using Net.Business.DTO.Informe;
using Net.Business.DTO.Planilla;
using Net.Business.DTO.Usuario;
using Net.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Net.Business.Services.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ApiExplorerSettings(GroupName = "ApiVenta")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class PlanillaController : ControllerBase
    {
        private readonly IRepositoryWrapper _repository;
        public PlanillaController(IRepositoryWrapper repository)
        {
            this._repository = repository;
        }

        /// <summary>
        /// Se invoca en: Botón buscar
        /// </summary>
        /// <param name="fecha1"></param>
        /// <param name="fecha2"></param>
        /// <param name="coduser"></param>
        /// <param name="codcentro"></param>
        /// <param name="orden"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetListaCuadredeCajaGeneralPorFiltro([FromQuery] string fecha1, string fecha2, string coduser, string codcentro, string orden)
        {

            var objectGetAll = await _repository.CuadreCaja.GetListaCuadredeCajaGeneralPorFiltro(fecha1, fecha2, coduser, codcentro, orden);

            if (objectGetAll.ResultadoCodigo == -1)
            {
                return BadRequest(objectGetAll);
            }

            var obj = new DtoCuadreCajaGeneralPlanillaListarResponse().RetornarListaCuadreCajaGeneral(objectGetAll.dataList);

            return Ok(obj.ListaCuadreCajaGeneral);
        }

        /// <summary>
        /// Se invoca en: Botón Buscar y evento click de grilla caja
        /// </summary>
        /// <param name="documento"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetListaCuadredeCajaPorFiltro([FromQuery] string documento)
        {
            var objectGetAll = await _repository.CuadreCaja.GetListaCuadredeCajaPorFiltro(documento);

            if (objectGetAll.ResultadoCodigo == -1)
            {
                return BadRequest(objectGetAll);
            }

            var obj = new DtoCuadreCajaPlanillaListarResponse().RetornarListaCuadreCaja(objectGetAll.dataList);

            return Ok(obj.ListaCuadreCaja);
        }


        /// <summary>
        /// Se invoca en: Botón procesar
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RegistrarPorUsuario([FromBody] DtoPlanillaRegistrarPorUsuario value)
        {
            try
            {
                if (value == null)
                {
                    return BadRequest(ModelState);
                }

                var response = await _repository.Planilla.RegistrarPorUsuario(value.RetornaPlanilla());

                if (response.ResultadoCodigo < 0)
                {

                    return BadRequest(response);
                }
                else
                {
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Hubo un error en al procesar : { ex.Message.ToString() }");
            }
        }


        /// <summary>
        /// Se invoca en: Botón cambiar fecha
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> cambiarFecha([FromBody] DtoPlanillaModificar value)
        {
            try
            {
                if (value == null)
                {
                    return BadRequest(ModelState);
                }

                value.campo = "fecha";

                var response = await _repository.Planilla.Modificar(value.RetornaPlanilla());

                if (response.IdRegistro < 0)
                {

                    return BadRequest(response);
                }
                else
                {
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Hubo un error en al procesar : { ex.Message.ToString() }");
            }
        }

        /// <summary>
        /// Se invoca en: Botón borrar
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> anular([FromBody] DtoPlanillaModificar value)
        {
            try
            {
                if (string.IsNullOrEmpty(value.numeroplanilla))
                {
                    return BadRequest(ModelState);
                }

                var response = await _repository.Planilla.Elminar(value.numeroplanilla);

                if (response.IdRegistro < 0)
                {

                    return BadRequest(response);
                }
                else
                {
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Hubo un error en al procesar : { ex.Message.ToString() }");
            }
        }

        /// <summary>
        /// Se envoca en: Boton borrar
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        //[HttpPatch]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status409Conflict)]
        //public async Task<IActionResult> Anular([FromBody] BE_AnulacionDocumento value)
        //{
        //    try
        //    {
        //        if (value == null)
        //        {
        //            return BadRequest(ModelState);
        //        }

        //        var response = await _repository.AnulacionDocumento.Anular(value);

        //        if (response.IdRegistro < 0)
        //        {

        //            return BadRequest(response);
        //        }
        //        else
        //        {
        //            return Ok(response);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest($"Hubo un error en al procesar : { ex.Message.ToString() }");
        //    }
        //}

        /// <summary>
        /// Se invoca en: Botón borrar
        /// </summary>
        /// <param name="buscar"></param>
        /// <param name="key"></param>
        /// <param name="numerolineas"></param>
        /// <param name="orden"></param>
        /// <param name="serie"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetPlanillasPorFiltro([FromQuery] string buscar, int key, int numerolineas, int orden, string serie)
        {
            var objectGetAll = await _repository.Planilla.GetPlanillasPorFiltro(buscar, key, numerolineas, orden, serie);

            if (objectGetAll.ResultadoCodigo == -1)
            {
                return BadRequest(objectGetAll);
            }

            var obj = new DtoPlanillaResponse().RetornaPlanilla1Response(objectGetAll.data);

            return Ok(obj);
        }

        /// <summary>
        /// Se invoca en: Load, Botón borrar, consultar, cambiar fecha, Combo Serie y retornar
        /// </summary>
        /// <param name="buscar"></param>
        /// <param name="key"></param>
        /// <param name="numerolineas"></param>
        /// <param name="orden"></param>
        /// <param name="serie"></param>
        /// <param name="codcentro"></param>
        /// <param name="idusuario"></param>
        /// <param name="numeroPlanilla"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetListaPlanillasPorFiltro([FromQuery] string buscar, int key, int numerolineas, int orden, string serie,string codcentro,string idusuario,string numeroPlanilla,string fechaInicio,string fechaFin)
        {
            var objectGetAll = await _repository.Planilla.GetListaPlanillasPorFiltro(buscar, key, numerolineas, orden, serie, codcentro, idusuario, numeroPlanilla, fechaInicio, fechaFin);

            if (objectGetAll.ResultadoCodigo == -1)
            {
                return BadRequest(objectGetAll);
            }

            var obj = new DtoPlanillaListarResponse().RetornaPlanillaListaResponse(objectGetAll.dataList);

            return Ok(obj.ListaPlanilla);
        }

        /// <summary>
        /// Se invoca en: Botón imprimir
        /// </summary>
        /// <param name="fechainicio"></param>
        /// <param name="fechafin"></param>
        /// <param name="coduser"></param>
        /// <param name="codcentro"></param>
        /// <param name="numeroplanilla"></param>
        /// <param name="dolares"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetRpCuadreCajaPorFiltro([FromQuery] string fechainicio, string fechafin, string coduser, string codcentro, string numeroplanilla, decimal dolares)
        {
            var objectGetAll = await _repository.CuadreCaja.GetRpCuadreCajaPorFiltro(fechainicio, fechafin, coduser, codcentro, numeroplanilla, dolares);

            if (objectGetAll.ResultadoCodigo == -1)
            {
                return BadRequest(objectGetAll);
            }

            var obj = new DtoRpCuadreCajaListarResponse().RetornarListaCuadreCaja(objectGetAll.dataList);

            return Ok(obj.ListaCuadreCaja);
        }

        /// <summary>
        /// Se invoca en: Botón imprimir detalle
        /// </summary>
        /// <param name="fechainicio"></param>
        /// <param name="fechafin"></param>
        /// <param name="coduserp"></param>
        /// <param name="codcentro"></param>
        /// <param name="numeroplanilla"></param>
        /// <param name="orden"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetRpCuadreCajaDetalladoPorFiltro([FromQuery] string fechainicio, string fechafin, string coduserp, string codcentro, string numeroplanilla, string orden)
        {
            var objectGetAll = await _repository.CuadreCaja.GetRpCuadreCajaDetalladoPorFiltro(fechainicio, fechafin, coduserp, codcentro, numeroplanilla, orden);

            if (objectGetAll.ResultadoCodigo == -1)
            {
                return BadRequest(objectGetAll);
            }

            var obj = new DtoRpCuadreCajaDetalladoListarResponse().RetornarListaCuadreCaja(objectGetAll.dataList);

            return Ok(obj.ListaCuadreCajaDetallado);
        }


        /// <summary>
        /// Se invoca en: txtCodCentro_LostFocus, Form_KeyDown y método Hoja
        /// </summary>
        /// <param name="codtabla"></param>
        /// <param name="buscar"></param>
        /// <param name="key"></param>
        /// <param name="numerolineas"></param>
        /// <param name="orden"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetTablaLogisticaPorFiltros([FromQuery] string codtabla, string buscar, int key, int numerolineas, int orden)
        {

            var objectGetAll = await _repository.Tabla.GetTablaLogisticaPorFiltros(codtabla, buscar, key, numerolineas, orden);

            if (objectGetAll.ResultadoCodigo == -1)
            {
                return BadRequest(objectGetAll);
            }

            return Ok(objectGetAll.data);
        }

        /// <summary>
        /// Se invoca: Para llenar la serie
        /// </summary>
        /// <param name="codtabla"></param>
        /// <param name="buscar"></param>
        /// <param name="key"></param>
        /// <param name="numerolineas"></param>
        /// <param name="orden"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetListTablaLogisticaPorFiltros([FromQuery] string codtabla, string buscar, int key, int numerolineas, int orden)
        {

            var objectGetAll = await _repository.Tabla.GetListTablaLogisticaPorFiltros(codtabla, buscar, key, numerolineas, orden);

            if (objectGetAll.ResultadoCodigo == -1)
            {
                return BadRequest(objectGetAll);
            }

            return Ok(objectGetAll.dataList);
        }

        /// <summary>
        /// Se invoca en: txtCodCentro_LostFocus y método Hoja
        /// </summary>
        /// <param name="buscar"></param>
        /// <param name="key"></param>
        /// <param name="numerolineas"></param>
        /// <param name="orden"></param>
        /// <returns></returns>
        //[HttpGet]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //public async Task<IActionResult> GetCentro([FromQuery] string buscar, int key, int numerolineas, int orden)
        //{

        //    var objectGetAll = await _repository.CentroCosto.GetCentro(buscar, key, numerolineas, orden);

        //    if (objectGetAll.ResultadoCodigo == -1)
        //    {
        //        return BadRequest(objectGetAll);
        //    }

        //    return Ok(objectGetAll.data);
        //}

        /// <summary>
        /// Se invoca en: txtCodUsuario_LostFocus
        /// </summary>
        /// <param name="nombre"></param>
        /// <param name="estadodf"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        //public async Task<IActionResult> GetUsuarioPorFiltro([FromQuery] string codtabla, string buscar, int key, int numerolineas, int orden)
        public async Task<IActionResult> GetUsuarioPersonaFiltroNombre([FromQuery] string nombre)
        {
            if (nombre == null) nombre = string.Empty;

             //var objectGetAll = await _repository.Usuario.GetUsuarioPorFiltro(codtabla, buscar, key, numerolineas, orden);
             var objectGetAll = await _repository.Usuario.GetUsuarioPersonaPorFiltroNombre(nombre);

            if (objectGetAll.ResultadoCodigo == -1)
            {
                return BadRequest(objectGetAll);
            }

            return Ok(objectGetAll.dataList);
        }

        /// <summary>
        /// Se invoca en: Imprimir_TextBox
        /// </summary>
        /// <param name="buscar"></param>
        /// <param name="key"></param>
        /// <param name="numerolineas"></param>
        /// <param name="orden"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetInformePorFiltro([FromQuery] string buscar, int key, int numerolineas, int orden)
        {
            var objectGetAll = await _repository.Informe.GetInformePorFiltro(buscar, key, numerolineas, orden);

            if (objectGetAll.ResultadoCodigo == -1)
            {
                return BadRequest(objectGetAll);
            }

            var obj = new DtoInformeResponse().RetornaCentroResponse(objectGetAll.data);

            return Ok(obj);
        }

        /// <summary>
        /// Se invoca en: Form_Activate
        /// </summary>
        /// <param name="idemodulo"></param>
        /// <param name="ideusuario"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPerfilUsuario([FromQuery] int idemodulo, int ideusuario)
        {

            var objectGetAll = await _repository.PerfilUsuario.GetPerfilUsuario(idemodulo, ideusuario);

            if (objectGetAll.ResultadoCodigo == -1)
            {
                return BadRequest(objectGetAll);
            }

            return Ok(objectGetAll.dataList);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CuadreCajaRegistrar([FromBody] DtoPlanillaTipoPagoRequest value)
        {
            try
            {
                if (value == null) return BadRequest(ModelState);
               
                decimal tipoCambio = 0;

                var objectGetAllTC = await _repository.TipoCambio.GetObtieneTipoCambio();

                if (objectGetAllTC.ResultadoCodigo == -1)
                {
                    return BadRequest(objectGetAllTC);
                }

                foreach (var item in objectGetAllTC.dataList)
                {
                    tipoCambio = item.Rate;
                }

                if (tipoCambio <= 0) tipoCambio = 1;
                //if (tipoCambio <= 0) return Ok(new { resultadoCodigo = -1, resultadoDescripcion = "NO TIENE CONFIGURADO EL TIPO DE CAMBIO" });

                foreach (var item in value.tipoPagos)
                {
                    item.tipoCambio = tipoCambio;
                }

                var response = await _repository.CuadreCaja.CuadreCajaRegistrar(value.RetornaListaCuadreCaja());

                if (response.ResultadoCodigo < 0)
                {
                    return BadRequest(response);
                }
                else
                {
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Hubo un error en al procesar : { ex.Message.ToString() }");
            }
        }

    }
}
