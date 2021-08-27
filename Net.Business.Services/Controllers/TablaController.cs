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
    [ApiExplorerSettings(GroupName = "ApiTabla")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class TablaController : ControllerBase
    {
        private readonly IRepositoryWrapper _repository;
        public TablaController(IRepositoryWrapper repository)
        {
            this._repository = repository;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetListTablaClinicaPorFiltros([FromQuery] string codtabla, string buscar, int key, int numerolineas, int orden)
        {

            var objectGetAll = await _repository.Tabla.GetListTablaClinicaPorFiltros(codtabla, buscar, key, numerolineas, orden);

            if (objectGetAll.ResultadoCodigo == -1)
            {
                return BadRequest(objectGetAll);
            }

            return Ok(objectGetAll.dataList);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetTablasClinicaPorFiltros([FromQuery] string codtabla, string buscar, int key, int numerolineas, int orden)
        {

            var objectGetAll = await _repository.Tabla.GetTablasClinicaPorFiltros(codtabla, buscar, key, numerolineas, orden);

            if (objectGetAll.ResultadoCodigo == -1)
            {
                return BadRequest(objectGetAll);
            }

            return Ok(objectGetAll.data);
        }

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
    }
}
