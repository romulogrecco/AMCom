namespace Questao5.Application.Commands.Requests
{
    public class MovimentoRequestModel
    {
        public Guid ChaveIdempotencia { get; set; }
        public int NumeroConta { get; set; }
        public string TipoMovimento { get; set; }
        public double Valor { get; set; }
    }
}
