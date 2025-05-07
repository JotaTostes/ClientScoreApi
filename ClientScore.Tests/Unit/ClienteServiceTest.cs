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
            // Arrange
            var request = new ClienteRequestDto
            {
                Nome = "John Doe",
                CPF = "12345678900",
                Email = "john.doe@example.com",
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

            // Act
            var (clienteInserido, erros) = await _clienteService.CadastrarClienteAsync(request);

            // Assert
            Assert.Null(clienteInserido);
            Assert.Equal(2, erros.Count);
            Assert.Contains("Nome é obrigatório.", erros);
            Assert.Contains("CPF inválido.", erros);
        }

        [Fact]
        public async Task CadastrarClienteAsync_ShouldAddCliente_WhenValidationPasses()
        {
            // Arrange
            var request = new ClienteRequestDto
            {
                Nome = "John Doe",
                CPF = "12345678900",
                Email = "john.doe@example.com",
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

            // Act
            var (clienteInserido, erros) = await _clienteService.CadastrarClienteAsync(request);

            // Assert
            Assert.NotNull(clienteInserido);
            Assert.Empty(erros);
            Assert.Equal(request.Nome, clienteInserido.Nome);
            Assert.Equal(request.Email, clienteInserido.Email);
            Assert.Equal(85, clienteInserido.Score);

            _clienteRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Cliente>()), Times.Once);
        }
    }
}
