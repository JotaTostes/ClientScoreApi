using Asp.Versioning;
using ClientScore.Application.DTOs;
using ClientScore.Application.Interfaces;
using ClientScore.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

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
        [ProducesResponseType(typeof(ClienteResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post([FromBody] ClienteRequestDto clienteDto)
        {
            var (resultado, erros) = await _clienteService.CadastrarClienteAsync(clienteDto);

            if (!erros.IsNullOrEmpty())
                return BadRequest(new { Mensagem = "Erro ao cadastrar cliente.", Erros = erros });

            return CreatedAtAction(nameof(Post), "", resultado);
        }
    }
}
