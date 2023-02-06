namespace Questao5.Domain;

public interface IMovimentoCommandStore
{
    Task RealizarMovimentacao(int numeroConta, string tipoMovimento, double valor);
}
