namespace Questao5.Domain;

public interface IMovimentoQueryStore
{
    Task<double> ObterSaldo(int numeroConta);
}
