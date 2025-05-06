using ClientScore.Domain.Entities;
using ClientScore.Infrastructure.Interfaces;
using ClientScore.Infrastructure.Repositories;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Repositories
{
    public class ClienteRepository : BaseRepository ,IClienteRepository
    {
        public ClienteRepository(IConfiguration configuration) : base(configuration) { }

        public async Task AddAsync(Cliente cliente)
        {
            await ExecuteTransactionAsync(async (conn, tran) =>
            {
                var cmd = new SqlCommand(@"
                INSERT INTO Clientes (Nome, DataNascimento, CPF, Email, RendimentoAnual, Estado, Telefone, Score)
                VALUES (@Nome, @DataNascimento, @CPF, @Email, @RendimentoAnual, @Estado, @Telefone, @Score)
            ", conn, tran);

                cmd.Parameters.AddWithValue("@Nome", cliente.Nome);
                cmd.Parameters.AddWithValue("@DataNascimento", cliente.DataNascimento);
                cmd.Parameters.AddWithValue("@CPF", cliente.CPF);
                cmd.Parameters.AddWithValue("@Email", cliente.Email);
                cmd.Parameters.AddWithValue("@RendimentoAnual", cliente.RendimentoAnual);
                cmd.Parameters.AddWithValue("@Estado", cliente.Estado);
                cmd.Parameters.AddWithValue("@Telefone", cliente.Telefone);
                cmd.Parameters.AddWithValue("@Score", cliente.Score);

                await cmd.ExecuteNonQueryAsync();
            });
        }

    }
}
