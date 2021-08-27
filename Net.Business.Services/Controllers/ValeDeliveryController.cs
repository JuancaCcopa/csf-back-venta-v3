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
    public class ValeDeliveryController : ControllerBase
    {
        private readonly IRepositoryWrapper _repository;
        public ValeDeliveryController(IRepositoryWrapper repository)
        {
            this._repository = repository;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetListValeDeliveryPorCodAtencion([FromQuery] string codatencion)
        {

            var objectGetAll = await _repository.ValeDelivery.GetListValeDeliveryPorCodAtencion(codatencion);

            if (objectGetAll.ResultadoCodigo == -1)
            {
                return BadRequest(objectGetAll);
            }

            return Ok(objectGetAll.dataList);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RegistrarValeDelivery([FromBody] DtoValeDeliveryRegistrar value)
        {
            try
            {
                if (value == null)
                {
                    return BadRequest(ModelState);
                }

                var response = await _repository.ValeDelivery.RegistrarValeDelivery(value.RetornaModelo());

                if (response.ResultadoCodigo == -1)
                {
                    return BadRequest(response);
                }
                else
                {
                    return Ok(response);
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
        public async Task<IActionResult> ModificarValeDelivery([FromBody] DtoValeDeliveryRegistrar value)
        {
            try
            {
                if (value == null)
                {
                    return BadRequest(ModelState);
                }

                var response = await _repository.ValeDelivery.ModificarValeDelivery(value.RetornaModelo());

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
