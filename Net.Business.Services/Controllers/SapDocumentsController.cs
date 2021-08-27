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
    [ApiExplorerSettings(GroupName = "ApiVenta")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class SapDocumentsController : ControllerBase
    {
        private readonly IRepositoryWrapper _repository;
        public SapDocumentsController(IRepositoryWrapper repository)
        {
            this._repository = repository;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SetCreateDocument([FromBody] DtoVentaSap value)
        {
            try
            {
                if (value == null)
                {
                    return BadRequest(ModelState);
                }

                var response = await _repository.SapDocuments.SetCreateDocument(value.RetornaVentaSap());

                if (response.ResultadoCodigo == -1)
                {
                    return BadRequest(response);
                }
                else
                {
                    return NoContent();
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Hubo un error en la solicitud : { ex.Message.ToString() }");
            }

        }
    }
}
