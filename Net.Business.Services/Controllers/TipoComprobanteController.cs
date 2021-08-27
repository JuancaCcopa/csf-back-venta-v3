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
    public class TipoComprobanteController : ControllerBase
    {
        private readonly IRepositoryWrapper _repository;
        public TipoComprobanteController(IRepositoryWrapper repository)
        {
            this._repository = repository;
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetListVentasTipoComprobantes()
        {

            var objectGetAll = await _repository.TipoComprobante.VentasTipoComprobantes();
            if (objectGetAll.ResultadoCodigo == -1)
            {
                return BadRequest(objectGetAll);
            }

            return Ok(objectGetAll.dataList);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> getSeriePorCodDocumento(string codDocument)
        {

            var objectGetAll = await _repository.TipoComprobante.getSeriePorCodDocumento(codDocument);
            if (objectGetAll.ResultadoCodigo == -1)
            {
                return BadRequest(objectGetAll);
            }

            return Ok(objectGetAll.dataList);
        }

        //getCorrelativoPorCodDocumento

    }
}
