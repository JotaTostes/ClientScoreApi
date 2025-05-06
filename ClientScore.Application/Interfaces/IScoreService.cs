using ClientScore.Application.DTOs;
using ClientScore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientScore.Application.Interfaces
{
    public interface IScoreService
    {
        int CalcularScore(ClienteRequestDto cliente);
    }
}
