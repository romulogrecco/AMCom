namespace Questao5.Domain;

public interface IIdempotenciaCommandStore
{
    Task RegistrarIdempotencia(Guid chaveIdempotencia, string requisicao, string resultado);
}
