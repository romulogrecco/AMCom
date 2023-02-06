using Dapper;
using Microsoft.Data.Sqlite;
using Questao5.Domain;
using Questao5.Infrastructure.Sqlite;

namespace Questao5.Infrastructure.Database.CommandStore
{
    public class IdempotenciaCommandStore : IIdempotenciaCommandStore
    {
        private readonly DatabaseConfig databaseConfig;

        public IdempotenciaCommandStore(DatabaseConfig databaseConfig)
        {
            this.databaseConfig = databaseConfig;
        }

        public async Task RegistrarIdempotencia(Guid chaveIdempotencia, string requisicao, string resultado)
        {
            using var connection = new SqliteConnection(databaseConfig.Name + ";foreign keys=false;");

            var sql = $@"
                INSERT INTO idempotencia (chave_idempotencia, requisicao, resultado)
                VALUES ('{chaveIdempotencia}', '{requisicao}', '{resultado}')
            ";

            await connection.ExecuteAsync(sql);
        }
    }
}
