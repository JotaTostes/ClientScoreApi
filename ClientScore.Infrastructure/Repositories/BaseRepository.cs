
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace ClientScore.Infrastructure.Repositories
{
    public abstract class BaseRepository
    {
        private readonly string _connectionString;

        protected BaseRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        protected async Task ExecuteTransactionAsync(Func<SqlConnection, SqlTransaction, Task> operation)
        {
            await using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var transaction = connection.BeginTransaction();
            try
            {
                await operation(connection, transaction);
                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        protected async Task<T> ExecuteTransactionAsync<T>(Func<SqlConnection, SqlTransaction, Task<T>> operation)
        {
            await using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var transaction = connection.BeginTransaction();
            try
            {
                var result = await operation(connection, transaction);
                transaction.Commit();
                return result;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
    }
}
