namespace Questao1;

public class ContaBancaria {
    public int Numero { get; }
    public string NomeTitular { get; private set; }
    public double Saldo { get; private set; }

    private readonly double TAXA_BANCARIA = 3.5;

    public ContaBancaria(int numero, string nomeTitular, double depositoInicial = 0)
    {
        Numero = numero;
        NomeTitular = nomeTitular;
        Saldo = DefinirSaldoInicial(depositoInicial);
    }

    // Para evitar de iniciar a conta com saldo negativo
    private double DefinirSaldoInicial(double valor)
    {
        if (valor > 0) return valor;

        return 0;
    }

    public void DefinirNomeTitular(string valor) => NomeTitular = valor;

    public void Sacar(double valor)
    {
        Saldo -= valor + TAXA_BANCARIA;
    }

    public void Depositar(double valor)
    {
        Saldo += valor;
    }
}
