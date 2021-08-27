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
    [ApiExplorerSettings(GroupName = "ApiMedico")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class MedicoController : ControllerBase
    {
        private readonly IRepositoryWrapper _repository;
        public MedicoController(IRepositoryWrapper repository)
        {
            this._repository = repository;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nombre"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetListMedicoPorNombre([FromQuery] string nombre)
        {

            var objectGetAll = await _repository.Medico.GetListMedicoPorNombre(nombre);

            if (objectGetAll == null)
            {
                return NotFound();
            }

            return Ok(objectGetAll);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="codAtencion"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetListMedicoPorAtencion([FromQuery] string codAtencion)
        {

            var objectGetAll = await _repository.Medico.GetListMedicoPorAtencion(codAtencion);

            if (objectGetAll == null)
            {
                return NotFound();
            }

            return Ok(objectGetAll);
        }
    }
}
