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
    [ApiExplorerSettings(GroupName = "ApiReceta")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class RecetaController : ControllerBase
    {
        private readonly IRepositoryWrapper _repository;
        public RecetaController(IRepositoryWrapper repository)
        {
            this._repository = repository;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetListRecetasPorFiltro([FromQuery] DateTime fechainicio, DateTime fechafin, string codtipoconsultamedica, int ide_receta, string nombrespaciente, string sbaestadoreceta)
        {

            var objectGetAll = await _repository.Receta.GetListRecetasPorFiltro(fechainicio, fechafin, codtipoconsultamedica, ide_receta, nombrespaciente, sbaestadoreceta);

            if (objectGetAll.ResultadoCodigo == -1)
            {
                return BadRequest(objectGetAll);
            }

            return Ok(objectGetAll.dataList);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetListRecetasObservacionPorReceta([FromQuery] int ide_receta)
        {

            var objectGetAll = await _repository.Receta.GetListRecetasObservacionPorReceta(ide_receta);

            if (objectGetAll.ResultadoCodigo == -1)
            {
                return BadRequest(objectGetAll);
            }

            return Ok(objectGetAll.dataList);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RegistrarRecetaObservacion([FromBody] DtoRecetaObservacionRegistrar value)
        {
            try
            {
                if (value == null)
                {
                    return BadRequest(ModelState);
                }

                var response = await _repository.Receta.RegistrarRecetasObservacion(value.RetornaModelo());

                if (response.ResultadoCodigo == -1)
                {
                    return BadRequest(response);
                }
                else
                {
                    return Ok(response.dataList);
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Hubo un error en la solicitud : { ex.Message.ToString() }");
            }

        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ModificarRecetasObservacion([FromBody] DtoRecetaObservacionModificar value)
        {
            try
            {
                if (value == null)
                {
                    return BadRequest(ModelState);
                }

                var response = await _repository.Receta.ModificarRecetasObservacion(value.RetornaModelo());

                if (response.ResultadoCodigo == -1)
                {
                    return BadRequest(response);
                }
                else
                {
                    return Ok(response.dataList);
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Hubo un error en la solicitud : { ex.Message.ToString() }");
            }

        }
    }
}
