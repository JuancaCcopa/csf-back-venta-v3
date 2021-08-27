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
    [ApiExplorerSettings(GroupName = "ApiPersonaClinica")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class PersonalClinicaController : ControllerBase
    {
        private readonly IRepositoryWrapper _repository;
        public PersonalClinicaController(IRepositoryWrapper repository)
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
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetListPersonalClinicaPorNombre([FromQuery] string nombre)
        {

            var objectGetAll = await _repository.PersonalClinica.GetListPersonalClinicaPorNombre(nombre);

            if (objectGetAll.ResultadoCodigo == -1)
            {
                return BadRequest(objectGetAll);
            }

            return Ok(objectGetAll.dataList);
        }
    }
}
