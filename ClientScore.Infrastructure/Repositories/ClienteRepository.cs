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
                INSERT INTO Clientes 
                (Id, Nome, Email, CPF, DataNascimento, RendimentoAnual, Telefone, Estado, Score)
                VALUES 
                (@Id, @Nome, @Email, @CPF, @DataNascimento, @RendimentoAnual, @Telefone, @Estado, @Score)
            ", conn, tran);

                cmd.Parameters.AddWithValue("@Id", cliente.Id);
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

        public async Task<IEnumerable<Cliente>> GetAllAsync()
        {
            var clientes = new List<Cliente>();

            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            var cmd = new SqlCommand("SELECT * FROM Clientes", conn);

            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                clientes.Add(new Cliente
                {
                    Id = reader.GetGuid(reader.GetOrdinal("Id")),
                    Nome = reader.GetString(reader.GetOrdinal("Nome")),
                    Email = reader.GetString(reader.GetOrdinal("Email")),
                    CPF = reader.GetString(reader.GetOrdinal("CPF")),
                    DataNascimento = reader.GetDateTime(reader.GetOrdinal("DataNascimento")),
                    RendimentoAnual = reader.GetDecimal(reader.GetOrdinal("RendimentoAnual")),
                    Telefone = reader.IsDBNull(reader.GetOrdinal("Telefone")) ? null : reader.GetString(reader.GetOrdinal("Telefone")),
                    Estado = reader.GetString(reader.GetOrdinal("Estado")),
                    Score = reader.GetInt32(reader.GetOrdinal("Score"))
                });
            }

            return clientes;
        }

        public async Task<Cliente?> GetByCpfAsync(string cpf)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var query = @"
                SELECT 
                    Id, Nome, Email, CPF, DataNascimento, 
                    RendimentoAnual, Telefone, Estado, Score
                FROM Clientes 
                WHERE CPF = @Cpf";

            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Cpf", cpf);

            using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new Cliente
                {
                    Id = reader.GetGuid(reader.GetOrdinal("Id")),
                    Nome = reader.GetString(reader.GetOrdinal("Nome")),
                    Email = reader.GetString(reader.GetOrdinal("Email")),
                    CPF = reader.GetString(reader.GetOrdinal("CPF")),
                    DataNascimento = reader.GetDateTime(reader.GetOrdinal("DataNascimento")),
                    RendimentoAnual = reader.GetDecimal(reader.GetOrdinal("RendimentoAnual")),
                    Telefone = reader.IsDBNull(reader.GetOrdinal("Telefone"))
                               ? null
                               : reader.GetString(reader.GetOrdinal("Telefone")),
                    Estado = reader.GetString(reader.GetOrdinal("Estado")),
                    Score = reader.GetInt32(reader.GetOrdinal("Score"))
                };
            }
            return null;
        }
    }
}
