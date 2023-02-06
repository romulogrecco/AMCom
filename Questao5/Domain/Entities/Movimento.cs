namespace Questao5.Domain.Entities;

public class Movimento
{
    public Guid IdMovimento { get; private set; }
    public Guid IdContaCorrente { get; private set; }
    public DateTime DataMovimento { get; private set; }
    public string TipoMovimento { get; private set; }
    public double Valor { get; private set; }
}
