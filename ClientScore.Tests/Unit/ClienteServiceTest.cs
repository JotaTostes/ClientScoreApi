using ClientScore.Application.DTOs;
using ClientScore.Application.Interfaces;
using ClientScore.Application.Mappers;
using ClientScore.Application.Services;
using ClientScore.Domain.Entities;
using ClientScore.Infrastructure.Interfaces;
using FluentValidation;
using FluentValidation.Results;
using Moq;

namespace ClientScore.Tests.Unit
{
    public class ClienteServiceTests
    {
        private readonly Mock<IClienteRepository> _clienteRepositoryMock;
        private readonly Mock<IScoreService> _scoreServiceMock;
        private readonly Mock<IValidator<ClienteRequestDto>> _validatorMock;
        private readonly ClienteService _clienteService;

        public ClienteServiceTests()
        {
            _clienteRepositoryMock = new Mock<IClienteRepository>();
            _scoreServiceMock = new Mock<IScoreService>();
            _validatorMock = new Mock<IValidator<ClienteRequestDto>>();
            _clienteService = new ClienteService(
                _clienteRepositoryMock.Object,
                _scoreServiceMock.Object,
                _validatorMock.Object
            );
        }

        [Fact]
        public async Task CadastrarClienteAsync_ShouldReturnErrors_WhenValidationFails()
        {
            var request = new ClienteRequestDto
            {
                Nome = "João",
                CPF = "12345678900",
                Email = "joao@gmail.com",
                DataNascimento = new System.DateTime(1990, 1, 1),
                RendimentoAnual = 50000,
                Estado = "SP",
                Telefone = "123456789"
            };

            var validationFailures = new List<ValidationFailure>
            {
                new ValidationFailure("Nome", "Nome é obrigatório."),
                new ValidationFailure("CPF", "CPF inválido.")
            };

            _validatorMock
                .Setup(v => v.ValidateAsync(request, default))
                .ReturnsAsync(new ValidationResult(validationFailures));

            var (clienteInserido, erros) = await _clienteService.CadastrarClienteAsync(request);

            Assert.Null(clienteInserido);
            Assert.Equal(2, erros.Count);
            Assert.Contains("Nome é obrigatório.", erros);
            Assert.Contains("CPF inválido.", erros);
        }

        [Fact]
        public async Task CadastrarClienteAsync_ShouldAddCliente_WhenValidationPasses()
        {
            var request = new ClienteRequestDto
            {
                Nome = "João",
                CPF = "12345678900",
                Email = "joao@gmail.com",
                DataNascimento = new System.DateTime(1990, 1, 1),
                RendimentoAnual = 50000,
                Estado = "SP",
                Telefone = "123456789"
            };

            var cliente = ClienteMapper.ToEntity(request);
            cliente.Score = 85;

            _validatorMock
                .Setup(v => v.ValidateAsync(request, default))
                .ReturnsAsync(new ValidationResult());

            _scoreServiceMock
                .Setup(s => s.CalcularScore(request))
                .Returns(85);

            _clienteRepositoryMock
                .Setup(r => r.AddAsync(It.IsAny<Cliente>()))
                .Returns(Task.CompletedTask);

            var (clienteInserido, erros) = await _clienteService.CadastrarClienteAsync(request);

            Assert.NotNull(clienteInserido);
            Assert.Empty(erros);
            Assert.Equal(request.Nome, clienteInserido.Nome);
            Assert.Equal(request.Email, clienteInserido.Email);
            Assert.Equal(85, clienteInserido.Score);

            _clienteRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Cliente>()), Times.Once);
        }

        [Fact]
        public async Task ObterTodosClientesAsync_DeveRetornarListaDeClientes()
        {
            var clientes = new List<Cliente>
        {
            new Cliente { Id = Guid.NewGuid(), Nome = "João", Email = "joao@gmail.com", CPF = "12345678900", Estado = "SP", Score = 450 },
            new Cliente { Id = Guid.NewGuid(), Nome = "Maria", Email = "maria@gmail.com", CPF = "98765432100", Estado = "RJ", Score = 300 }
        };

            _clienteRepositoryMock
                .Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(clientes);

            var resultado = await _clienteService.ObterTodosClientesAsync();

            Assert.NotNull(resultado);
            Assert.Equal(2, resultado.Count);
            Assert.Equal("João", resultado[0].Nome);
        }

        [Fact]
        public async Task ObterPorCpfAsync_ClienteExiste_DeveRetornarCliente()
        {
            var cliente = new Cliente { Id = Guid.NewGuid(), Nome = "João", Email = "joao@mail.com", CPF = "12345678900", Estado = "SP", Score = 300 };

            _clienteRepositoryMock
                .Setup(repo => repo.GetByCpfAsync("12345678900"))
                .ReturnsAsync(cliente);

            var resultado = await _clienteService.ObterPorCpfAsync("12345678900");

            Assert.NotNull(resultado);
            Assert.Equal("João", resultado!.Nome);
        }

        [Fact]
        public async Task ObterPorCpfAsync_ClienteNaoExiste_DeveRetornarNull()
        {
            _clienteRepositoryMock
                .Setup(repo => repo.GetByCpfAsync("00000000000"))
                .ReturnsAsync((Cliente?)null);

            var resultado = await _clienteService.ObterPorCpfAsync("00000000000");

            Assert.Null(resultado);
        }
    }
}
