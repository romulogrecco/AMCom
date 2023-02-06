using Questao5.Domain.Entities;

namespace Questao5.Domain;

public interface IContaCorrenteQueryStore
{
    Task<ContaCorrente> ObterPeloNumero(int numeroConta);
}
