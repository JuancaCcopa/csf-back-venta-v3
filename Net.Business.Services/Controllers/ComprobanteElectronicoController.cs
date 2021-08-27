using System;
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
    [ApiExplorerSettings(GroupName = "ApiSerie")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class ComprobanteElectronicoController : ControllerBase
    {
        private readonly IRepositoryWrapper _repository;
        public ComprobanteElectronicoController(IRepositoryWrapper repository)
        {
            this._repository = repository;
        }

        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetListComprobanteElectronicoPorFiltro([FromQuery] string codempresa, string codcomprobante, string codcomprobante_e, string codsistema, string tipocomp_sunat, int orden)
        {

            var objectGetAll = await _repository.ComprobanteElectronico.GetListComprobanteElectronicoPorFiltro(codempresa, codcomprobante, codcomprobante_e, codsistema, tipocomp_sunat, orden);

            if (objectGetAll.ResultadoCodigo == -1)
            {
                return BadRequest(objectGetAll);
            }

            return Ok(objectGetAll.dataList);
        }
    }
}
