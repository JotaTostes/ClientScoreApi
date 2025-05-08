using Asp.Versioning;
using ClientScore.Application.DTOs;
using ClientScore.Application.Interfaces;
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
        /// Cadastrar um cliente e aplicar regra de validação de score.
        /// </summary>
        /// <param name="clienteDto">Dados do cliente para cadastrar</param>
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

        /// <summary>
        /// Retorna uma lista de todos clientes cadastrados
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAll")]
        [ProducesResponseType(typeof(List<ClienteListagemDto>), 200)]
        public async Task<IActionResult> GetAll()
        {
            var clientes = await _clienteService.ObterTodosClientesAsync();
            return Ok(clientes);
        }

        /// <summary>
        /// Retorna cliente filtrando por CPF
        /// </summary>
        /// <param name="cpf"></param>
        /// <returns></returns>
        [HttpGet("{cpf}")]
        [ProducesResponseType(typeof(ClienteListagemDto), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetByCpf(string cpf)
        {
            var cliente = await _clienteService.ObterPorCpfAsync(cpf);

            return cliente is null
                ? NotFound(new { Mensagem = "Cliente não encontrado." })
                : Ok(cliente);
        }
    }
}
