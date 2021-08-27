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
    [ApiExplorerSettings(GroupName = "ApiHospital")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class HospitalController : ControllerBase
    {
        private readonly IRepositoryWrapper _repository;
        public HospitalController(IRepositoryWrapper repository)
        {
            this._repository = repository;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetHospitalDatosPorAtencion([FromQuery] string codatencion)
        {

            var objectGetAll = await _repository.Hospital.GetHospitalDatosPorAtencion(codatencion);

            if (objectGetAll == null)
            {
                return NotFound();
            }

            return Ok(objectGetAll);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetListHospitalExclusionesPorAtencion([FromQuery] string codatencion)
        {

            var objectGetAll = await _repository.Hospital.GetListHospitalExclusionesPorAtencion(codatencion);

            if (objectGetAll.ResultadoCodigo == -1)
            {
                return BadRequest(objectGetAll);
            }

            return Ok(objectGetAll.dataList);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetListHospitalPacienteClinicaPorFiltros([FromQuery] string pabellon, string piso, string local)
        {

            var objectGetAll = await _repository.Hospital.GetListHospitalPacienteClinicaPorFiltros(pabellon, piso, local);

            if (objectGetAll.ResultadoCodigo == -1)
            {
                return BadRequest(objectGetAll);
            }

            return Ok(objectGetAll.dataList);
        }
    }
}
