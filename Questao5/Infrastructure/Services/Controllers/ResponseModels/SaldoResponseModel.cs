namespace Questao5.Infrastructure.Services.Controllers.ResponseModels
{
    public class SaldoResponseModel
    {
        public int NumeroConta { get; set; }
        public string NomeTitularConta { get; set; }
        public DateTime Resposta { get; set; }
        public double Saldo { get; set; }

        public SaldoResponseModel(int numeroConta, string nomeTitularConta, double saldo)
        {
            NumeroConta = numeroConta;
            NomeTitularConta = nomeTitularConta;
            Resposta = DateTime.Now;
            Saldo = saldo;
        }
    }
}
