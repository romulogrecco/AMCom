using Dapper;
using Microsoft.Data.Sqlite;
using Questao5.Domain;
using Questao5.Infrastructure.Sqlite;

namespace Questao5.Infrastructure.Database.QueryStore
{
    public class MovimentoQueryStore : IMovimentoQueryStore
    {
        private readonly DatabaseConfig databaseConfig;

        public MovimentoQueryStore(DatabaseConfig databaseConfig)
        {
            this.databaseConfig = databaseConfig;
        }

        public async Task<double> ObterSaldo(int numeroConta)
        {
            using var connection = new SqliteConnection(databaseConfig.Name);

            var sql = $@"
                SELECT SUM(CASE WHEN tipomovimento = 'C' THEN valor WHEN tipomovimento = 'D'THEN -valor ELSE 0 END) AS saldo
                FROM movimento m 
                WHERE idcontacorrente = {numeroConta}
                GROUP BY idcontacorrente
            ";

            var resultado = await connection.QueryAsync<double>(sql);

            return resultado.FirstOrDefault();
        }
    }
}
