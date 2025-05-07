using ClientScore.Application.DTOs;

namespace ClientScore.Application.Interfaces
{
    public interface IScoreService
    {
        int CalcularScore(ClienteRequestDto cliente);
        int CalcularIdade(DateTime dataNascimento);
    }
}
