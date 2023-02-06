using FluentValidation;

namespace Questao5.Application.Commands;

public class RealizarMovimentacaoCommand : Command
{
    public Guid ChaveIdempotencia { get; private set; }
    public int NumeroConta { get; private set; }
    public string TipoMovimento { get; private set; }
    public double Valor { get; private set; }

    public RealizarMovimentacaoCommand( Guid chaveIdempotencia, int numeroConta, string tipoMovimento, double valor)
    {
        EntityId = chaveIdempotencia;
        ChaveIdempotencia = chaveIdempotencia;
        NumeroConta = numeroConta;
        TipoMovimento = tipoMovimento;
        Valor = valor;
    }

    public override bool EhValido()
    {
        ValidationResult = new RealizarMovimentacaoValidation().Validate(this);
        return ValidationResult.IsValid;
    }
}

public class RealizarMovimentacaoValidation : AbstractValidator<RealizarMovimentacaoCommand>
{
    public RealizarMovimentacaoValidation()
    {
        RuleFor(c => c.ChaveIdempotencia).NotEqual(Guid.Empty).WithMessage("Chave de idempotencia inválido");
        RuleFor(c => c.NumeroConta).GreaterThan(0).WithMessage("Número da conta inválido");
        RuleFor(c => c.TipoMovimento).Must(x => new List<string>{ "C", "D" }.Contains(x)).WithMessage("Tipo de movimentação inválida");
        RuleFor(c => c.Valor).GreaterThan(0).WithMessage("O valor deve ser maior que zero");
    }
}