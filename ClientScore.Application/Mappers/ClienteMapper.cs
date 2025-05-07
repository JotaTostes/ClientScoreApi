using ClientScore.Application.DTOs;
using ClientScore.Application.Validator;
using ClientScore.Domain.Entities;

namespace ClientScore.Application.Mappers
{
    public static class ClienteMapper
    {
        public static Cliente ToEntity(ClienteRequestDto request)
        {
            return new Cliente
            {
                Nome = request.Nome,
                DataNascimento = request.DataNascimento,
                CPF = request.CPF,
                Email = request.Email,
                RendimentoAnual = request.RendimentoAnual,
                Estado = request.Estado.ToUpper(),
                Telefone = TelefoneValidator.Normalize(request.Telefone),
            };
        }

        public static ClienteResponseDto ToResponse(Cliente cliente)
        {
            return new ClienteResponseDto
            {
                Nome = cliente.Nome,
                Email = cliente.Email,
                Score = cliente.Score
            };
        }
    }
}
