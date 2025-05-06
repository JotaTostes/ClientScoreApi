using Asp.Versioning;
using ClientScore.Application.DTOs;
using ClientScore.Application.Interfaces;
using ClientScore.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/cliente")] 
    public class ClienteController : ControllerBase
    {
        private readonly IClienteService _clienteService;

        public ClienteController(IClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        /// <summary>
        /// Cadastrar um cliente
        /// </summary>
        /// <param name="cliente"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ClienteResponseDto), 200)]
        public async Task<IActionResult> Post([FromBody] ClienteRequestDto cliente)
        {
            var resultado = await _clienteService.CadastrarClienteAsync(cliente);
            return Created("", resultado);
        }
    }
}
