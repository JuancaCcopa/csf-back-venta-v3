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
    [ApiExplorerSettings(GroupName = "ApiComprobante")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class ComprobanteController : ControllerBase
    {
        private readonly IRepositoryWrapper _repository;
        public ComprobanteController(IRepositoryWrapper repository)
        {
            this._repository = repository;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetListaComprobantesPorFiltro([FromQuery] string codcomprobante, DateTime fecinicio, DateTime fecfin, int opcion)
        {

            var objectGetAll = await _repository.Comprobante.GetListaComprobantesPorFiltro(codcomprobante, fecinicio, fecfin, opcion);

            if (objectGetAll.ResultadoCodigo == -1)
            {
                return BadRequest(objectGetAll);
            }

            var obj = new DtoComprobanteListarResponse().RetornarListaComprobante(objectGetAll.dataList);

            return Ok(obj.ListaVentaCabecera);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetComprobantesElectronico([FromQuery] string codcomprobantee, string codsistema)
        {

            var objectComprobante = await _repository.Comprobante.GetComprobantesElectronico(codcomprobantee, codsistema);
            //objectComprobante.
            if (objectComprobante.ResultadoCodigo == -1) return BadRequest(objectComprobante);
            if (objectComprobante.data.codcomprobante == null) return BadRequest($"No existe el comprobante {codcomprobantee}");
            var obj = new DtoComprobanteResponse().RetornaDtoComprobanteResponse(objectComprobante.data);
            return Ok(obj);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetListaCuadreCaja([FromQuery] string documento)
        {
            var objectGetAll = await _repository.Comprobante.GetListaCuadreCaja(documento.Trim());

            if (objectGetAll.ResultadoCodigo == -1)
            {
                return BadRequest(objectGetAll);
            }

            var obj = new DtoComprobanteListaTipoPagoResponse().RetornarListaCuadreCaja(objectGetAll.dataList);
            return Ok(obj.lista);

        }
    }
}
