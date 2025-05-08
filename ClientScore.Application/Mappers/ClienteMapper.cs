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
                Id = cliente.Id,
                Nome = cliente.Nome,
                Email = cliente.Email,
                Score = cliente.Score
            };
        }

        public static List<ClienteListagemDto> ToListClientes(IEnumerable<Cliente> clientes)
        {
            return clientes.Select(c => new ClienteListagemDto
            {
                Id = c.Id,
                Nome = c.Nome,
                Email = c.Email,
                CPF = c.CPF,
                DataNascimento = c.DataNascimento,
                Telefone = c.Telefone,
                Estado = c.Estado
            }).ToList();
        }

        public static ClienteListagemDto ToSingleCliente(Cliente cliente)
        {
            return new ClienteListagemDto
            {
                Id = cliente.Id,
                Nome = cliente.Nome,
                Email = cliente.Email,
                CPF = cliente.CPF,
                DataNascimento = cliente.DataNascimento,
                Telefone = cliente.Telefone,
                Estado = cliente.Estado
            };
        }
    }
}
