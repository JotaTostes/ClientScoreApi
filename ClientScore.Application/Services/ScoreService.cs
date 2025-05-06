using ClientScore.Application.DTOs;
using ClientScore.Application.Interfaces;
using ClientScore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientScore.Application.Services
{
    public class ScoreService : IScoreService
    {
        public int CalcularScore(ClienteRequestDto cliente)
        {
            var score = 0;

            if (cliente.RendimentoAnual > 120000)
                score += 300;
            else if (cliente.RendimentoAnual >= 60000)
                score += 200;
            else
                score += 100;

            var idade = DateTime.Today.Year - cliente.DataNascimento.Year;
            if (cliente.DataNascimento.Date > DateTime.Today.AddYears(-idade)) idade--;

            if (idade > 40)
                score += 200;
            else if (idade >= 25)
                score += 150;
            else
                score += 50;

            return score;
        }
    }
}
