using ClientScore.Application.DTOs;
using ClientScore.Application.Interfaces;

namespace ClientScore.Application.Services
{
    public class ScoreService : IScoreService
    {
        private const int PontuacaoRendaAlta = 300;
        private const int PontuacaoRendaMedia = 200;
        private const int PontuacaoRendaBaixa = 100;

        private const int PontuacaoIdadeAcima40 = 200;
        private const int PontuacaoIdadeEntre25E40 = 150;
        private const int PontuacaoIdadeAbaixo25 = 50;

        public int CalcularScore(ClienteRequestDto cliente)
        {
            if (cliente == null)
                throw new ArgumentNullException(nameof(cliente), "O objeto ClienteRequestDto não pode ser nulo.");

            var score = CalcularPontuacaoPorRenda(cliente.RendimentoAnual);
            score += CalcularPontuacaoPorIdade(cliente.DataNascimento);

            return score;
        }

        /// <summary>
        /// Calcula a pontuação com base na renda anual
        /// </summary>
        /// <param name="rendimentoAnual"></param>
        /// <returns></returns>
        private int CalcularPontuacaoPorRenda(decimal rendimentoAnual)
        {
            if (rendimentoAnual > 120000)
                return PontuacaoRendaAlta;
            if (rendimentoAnual >= 60000)
                return PontuacaoRendaMedia;
            return PontuacaoRendaBaixa;
        }

        /// <summary>
        /// Calcula a pontuação com base na idade
        /// </summary>
        /// <param name="dataNascimento"></param>
        /// <returns></returns>
        private int CalcularPontuacaoPorIdade(DateTime dataNascimento)
        {
            var idade = CalcularIdade(dataNascimento);

            if (idade > 40)
                return PontuacaoIdadeAcima40;
            if (idade >= 25)
                return PontuacaoIdadeEntre25E40;
            return PontuacaoIdadeAbaixo25;
        }

        /// <summary>
        /// Método auxiliar para calcular a idade com base na data de nascimento
        /// </summary>
        /// <param name="dataNascimento"></param>
        /// <returns></returns>
        public int CalcularIdade(DateTime dataNascimento)
        {
            var hoje = DateTime.Today;
            var idade = hoje.Year - dataNascimento.Year;
            if (dataNascimento.Date > hoje.AddYears(-idade)) idade--;
            return idade;
        }
    }
}
