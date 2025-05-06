using ClientScore.Application.DTOs;
using ClientScore.Application.Interfaces;
using ClientScore.Application.Mappers;
using ClientScore.Domain.Entities;
using ClientScore.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientScore.Application.Services
{
    public class ClienteService : IClienteService
    {
        private readonly IClienteRepository _clienteRepository;
        private readonly IScoreService _scoreService;

        public ClienteService(IClienteRepository clienteRepository, IScoreService scoreService)
        {
            _clienteRepository = clienteRepository;
            _scoreService = scoreService;
        }

        public async Task<ClienteResponseDto> CadastrarClienteAsync(ClienteRequestDto request)
        {
            var cliente = ClienteMapper.ToEntity(request);
            cliente.Score = _scoreService.CalcularScore(request);

            await _clienteRepository.AddAsync(cliente);
            return ClienteMapper.ToResponse(cliente);
        }
    }
}
