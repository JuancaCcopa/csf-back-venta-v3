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
    [ApiExplorerSettings(GroupName = "ApiAlmacen")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class WarehousesController : ControllerBase
    {
        private readonly IRepositoryWrapper _repository;
        public WarehousesController(IRepositoryWrapper repository)
        {
            this._repository = repository;
        }

        /// <summary>
        /// Lista los almacenes que tienen la palabra filtrada
        /// </summary>
        /// <param name="warehouseName">palabra filtrada</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetListWarehousesContains([FromQuery] string warehouseName)
        {

            var objectGetAll = await _repository.Warehouses.GetListWarehousesContains(warehouseName);

            if (objectGetAll == null)
            {
                return NotFound();
            }

            return Ok(objectGetAll);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="warehouseCode"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetWarehousesPorCodigo([FromQuery] string warehouseCode)
        {

            var objectGetAll = await _repository.Warehouses.GetWarehousesPorCodigo(warehouseCode);

            if (objectGetAll == null)
            {
                return NotFound();
            }

            return Ok(objectGetAll);
        }
    }
}
