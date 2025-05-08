using ClientScore.Application.DTOs;
using ClientScore.Application.Interfaces;
using ClientScore.Application.Mappers;
using ClientScore.Infrastructure.Interfaces;
using FluentValidation;

namespace ClientScore.Application.Services
{
    public class ClienteService : IClienteService
    {
        private readonly IClienteRepository _clienteRepository;
        private readonly IScoreService _scoreService;
        private readonly IValidator<ClienteRequestDto> _validator;

        public ClienteService(IClienteRepository clienteRepository, IScoreService scoreService, IValidator<ClienteRequestDto> validator)
        {
            _clienteRepository = clienteRepository;
            _scoreService = scoreService;
            _validator = validator;
        }

        public async Task<(ClienteResponseDto? clienteInserido, List<string> Erros)> CadastrarClienteAsync(ClienteRequestDto request)
        {
            var resultadoValidacao = await _validator.ValidateAsync(request);

            if (!resultadoValidacao.IsValid)
                return (null, resultadoValidacao.Errors.Select(e => e.ErrorMessage).ToList());

            var cliente = ClienteMapper.ToEntity(request);

            cliente.Score = _scoreService.CalcularScore(request);
            await _clienteRepository.AddAsync(cliente);

            return (ClienteMapper.ToResponse(cliente), new List<string>());
        }

        public async Task<List<ClienteListagemDto>> ObterTodosClientesAsync()
        {
            var clientes = await _clienteRepository.GetAllAsync();

            return ClienteMapper.ToListClientes(clientes);
        }

        public async Task<ClienteListagemDto?> ObterPorCpfAsync(string cpf)
        {
            var cliente = await _clienteRepository.GetByCpfAsync(cpf);

            if (cliente is null)
                return null;

            return ClienteMapper.ToSingleCliente(cliente);
        }
    }
}
