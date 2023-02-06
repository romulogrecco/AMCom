using Dapper;
using Microsoft.Data.Sqlite;
using Questao5.Domain;
using Questao5.Domain.Entities;
using Questao5.Infrastructure.Sqlite;

namespace Questao5.Infrastructure.Database.QueryStore;

public class ContaCorrenteQueryStore : IContaCorrenteQueryStore
{
    private readonly DatabaseConfig databaseConfig;

    public ContaCorrenteQueryStore(DatabaseConfig databaseConfig)
    {
        this.databaseConfig = databaseConfig;
    }

    public async Task<ContaCorrente> ObterPeloNumero(int numeroConta)
    {
        using var connection = new SqliteConnection(databaseConfig.Name);

        var sql = $@"
                SELECT idContaCorrente, numero, nome, ativo FROM contacorrente
                WHERE numero = {numeroConta}
            ";

        var resultado = await connection.QueryAsync<ContaCorrente>(sql);

        return resultado.FirstOrDefault();
    }
}
