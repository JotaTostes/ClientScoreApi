using ClientScore.Application.DTOs;
using ClientScore.Application.Services;

namespace ClientScore.Tests.Unit
{
    public class ScoreServiceTests
    {
        private readonly ScoreService _scoreService;

        public ScoreServiceTests()
        {
            _scoreService = new ScoreService();
        }

        [Fact]
        public void CalcularScore_DeveLancarExcecao_QuandoClienteForNulo()
        {
            ClienteRequestDto cliente = null;

            var exception = Assert.Throws<ArgumentNullException>(() => _scoreService.CalcularScore(cliente));
            Assert.Equal("O objeto ClienteRequestDto não pode ser nulo. (Parameter 'cliente')", exception.Message);
        }

        [Theory]
        [InlineData(130000, 450)] // Renda alta
        [InlineData(80000, 350)]  // Renda média
        [InlineData(50000, 250)]  // Renda baixaS
        public void CalcularPontuacaoPorRenda_DeveRetornarPontuacaoCorreta(decimal rendimentoAnual, int pontuacaoEsperada)
        {
            var pontuacao = _scoreService.CalcularScore(new ClienteRequestDto
            {
                RendimentoAnual = rendimentoAnual,
                DataNascimento = new DateTime(1990, 1, 1)
            });

            Assert.Equal(pontuacaoEsperada, pontuacao);
        }

        [Theory]
        [InlineData("1980-01-01", 300)] // Idade > 40
        [InlineData("1995-01-01", 250)] // 25 e 40
        [InlineData("2005-01-01", 150)]  // Idade < 25
        public void CalcularPontuacaoPorIdade_DeveRetornarPontuacaoCorreta(string dataNascimento, int pontuacaoEsperada)
        {
            var cliente = new ClienteRequestDto
            {
                DataNascimento = DateTime.Parse(dataNascimento),
                RendimentoAnual = 50000 
            };

            var pontuacao = _scoreService.CalcularScore(cliente);

            Assert.Equal(pontuacaoEsperada, pontuacao);
        }

        [Theory]
        [InlineData("1980-01-01", 130000, 500)] // Idade > 40 e Renda alta
        [InlineData("1995-01-01", 80000, 350)]  // 25 <= Idade <= 40 e Renda média
        [InlineData("2005-01-01", 50000, 150)]  // Idade < 25 e Renda baixa
        public void CalcularScore_DeveRetornarSomaDasPontuacoes(string dataNascimento, decimal rendimentoAnual, int pontuacaoEsperada)
        {
            var cliente = new ClienteRequestDto
            {
                DataNascimento = DateTime.Parse(dataNascimento),
                RendimentoAnual = rendimentoAnual
            };

            var pontuacao = _scoreService.CalcularScore(cliente);

            Assert.Equal(pontuacaoEsperada, pontuacao);
        }

        [Theory]
        [InlineData("1980-06-01", 44)] // Idade 44 pois data do aniversario não ocorreu ainda
        [InlineData("2000-01-01", 25)]
        [InlineData("2010-01-01", 15)]
        public void CalcularIdade_DeveRetornarIdadeCorreta(string dataNascimento, int idadeEsperada)
        {
            var data = DateTime.Parse(dataNascimento);

            var idade = _scoreService.CalcularIdade(data);

            Assert.Equal(idadeEsperada, idade);
        }
    }
}
