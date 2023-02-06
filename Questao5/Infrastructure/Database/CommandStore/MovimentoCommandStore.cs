using Dapper;
using Microsoft.Data.Sqlite;
using Questao5.Domain;
using Questao5.Infrastructure.Sqlite;
using System.Globalization;

namespace Questao5.Infrastructure.Database.CommandStore
{
    public class MovimentoCommandStore : IMovimentoCommandStore
    {
        private readonly DatabaseConfig databaseConfig;

        public MovimentoCommandStore(DatabaseConfig databaseConfig)
        {
            this.databaseConfig = databaseConfig;
        }

        public async Task RealizarMovimentacao(int numeroConta, string tipoMovimento, double valor)
        {
            using var connection = new SqliteConnection(databaseConfig.Name + ";foreign keys=false;");

            var valorString = valor.ToString("0.00", CultureInfo.InvariantCulture);

            var sql = $@"
                INSERT INTO movimento (idmovimento, idcontacorrente, datamovimento, tipomovimento , valor)
                VALUES ('{Guid.NewGuid()}', {numeroConta}, '{DateTime.Now}', '{tipoMovimento}', {valorString})
            ";

            await connection.ExecuteAsync(sql);
        }
    }
}
