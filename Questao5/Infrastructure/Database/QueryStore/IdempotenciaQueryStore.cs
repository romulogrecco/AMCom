using Dapper;
using Microsoft.Data.Sqlite;
using Questao5.Domain;
using Questao5.Infrastructure.Sqlite;

namespace Questao5.Infrastructure.Database.QueryStore;

public class IdempotenciaQueryStore : IIdempotenciaQueryStore
{
    private readonly DatabaseConfig databaseConfig;

    public IdempotenciaQueryStore(DatabaseConfig databaseConfig)
    {
        this.databaseConfig = databaseConfig;
    }

    public async Task<bool> VerificarExistente(Guid chave)
    {
        using var connection = new SqliteConnection(databaseConfig.Name);

        var sql = $@"
                SELECT chave_idempotencia, requisicao, resultado FROM idempotencia
                WHERE chave_idempotencia = '{chave}'
            ";

        var resultado = await connection.QueryAsync(sql);

        return resultado.Count() > 0;
    }
}
