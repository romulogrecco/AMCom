using MediatR;
using Newtonsoft.Json;
using Questao5.Application.Commands;
using Questao5.Application.Messages;
using Questao5.Domain;

namespace Questao5.Application.Handlers
{
    public class MovimentoHandler : IRequestHandler<RealizarMovimentacaoCommand, bool>
    {
        private readonly IMovimentoCommandStore _movimentoCommandStore;
        private readonly IIdempotenciaCommandStore _idempotenciaCommandStore;
        private readonly IIdempotenciaQueryStore _idempotenciaQueryStore;
        private readonly IContaCorrenteQueryStore _contaCorrenteQueryStore;
        private readonly IMediatorHandler _mediatorHandler;

        public MovimentoHandler(IMovimentoCommandStore movimentoCommandStore,
                                       IIdempotenciaQueryStore idempotenciaQueryStore,
                                       IIdempotenciaCommandStore idempotenciaCommandStore,
                                       IMediatorHandler mediatorHandler,
                                       IContaCorrenteQueryStore contaCorrenteQueryStore)
        {
            _movimentoCommandStore = movimentoCommandStore;
            _idempotenciaQueryStore = idempotenciaQueryStore;
            _idempotenciaCommandStore = idempotenciaCommandStore;
            _mediatorHandler = mediatorHandler;
            _contaCorrenteQueryStore = contaCorrenteQueryStore;
        }

        public async Task<bool> Handle(RealizarMovimentacaoCommand message, CancellationToken cancellationToken)
        {
            if (!ValidarComando(message)) return false;

            var conta = await _contaCorrenteQueryStore.ObterPeloNumero(message.NumeroConta);

            if (conta is null)
            {
                await _mediatorHandler.PublicarNotificacao(new DomainNotification("INVALID_ACCOUNT", $"A conta informada não existe"));
                return false;
            }
            else if (!conta.Ativo)
            {
                await _mediatorHandler.PublicarNotificacao(new DomainNotification("INACTIVE_ACCOUNT", $"A conta informada está inativa"));
                return false;
            }

            var idempotenciaExistente = await _idempotenciaQueryStore.VerificarExistente(message.ChaveIdempotencia);

            if (!idempotenciaExistente)
            {
                var tasks = new List<Task>();

                tasks.Add(_movimentoCommandStore.RealizarMovimentacao(message.NumeroConta, message.TipoMovimento, message.Valor));

                // ToDo: Enviar "request" 
                tasks.Add(_idempotenciaCommandStore.RegistrarIdempotencia(message.ChaveIdempotencia, "request", JsonConvert.SerializeObject(message)));

                await Task.WhenAll(tasks);           
            }

            // Retornar chave do registro;
            return true;
        }

        private bool ValidarComando(Command message)
        {
            if (message.EhValido()) return true;

            foreach (var error in message.ValidationResult.Errors)
            {
                _mediatorHandler.PublicarNotificacao(new DomainNotification(message.MessageType, error.ErrorMessage));
            }

            return false;
        }
    }
}
