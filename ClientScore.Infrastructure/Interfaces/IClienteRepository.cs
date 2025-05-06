using ClientScore.Domain.Entities;

namespace ClientScore.Infrastructure.Interfaces
{
    public interface IClienteRepository
    {
        Task AddAsync(Cliente cliente);
    }
}
