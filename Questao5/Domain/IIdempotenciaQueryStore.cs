namespace Questao5.Domain;

public interface IIdempotenciaQueryStore
{
    Task<bool> VerificarExistente(Guid chave);
}
