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
    [ApiExplorerSettings(GroupName = "ApiAtencion")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class AtencionController : ControllerBase
    {
        private readonly IRepositoryWrapper _repository;
        public AtencionController(IRepositoryWrapper repository)
        {
            this._repository = repository;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="opcion"></param>
        /// <param name="codpaciente"></param>
        /// <param name="nombres"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetListPacientePorFiltros([FromQuery] string opcion, string codpaciente, string nombres)
        {

            var objectGetAll = await _repository.Atencion.GetListPacientePorFiltros(opcion, codpaciente, nombres);

            if (objectGetAll == null)
            {
                return NotFound();
            }

            return Ok(objectGetAll);
        }
    }
}
