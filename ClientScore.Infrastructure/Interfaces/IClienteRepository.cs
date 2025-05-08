using ClientScore.Domain.Entities;

namespace ClientScore.Infrastructure.Interfaces
{
    public interface IClienteRepository
    {
        Task AddAsync(Cliente cliente);
        Task<IEnumerable<Cliente>> GetAllAsync();
        Task<Cliente?> GetByCpfAsync(string cpf);

    }
}
