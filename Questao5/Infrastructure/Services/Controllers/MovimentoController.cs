using MediatR;
using Microsoft.AspNetCore.Mvc;
using Questao5.Application.Commands;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Handlers;
using Questao5.Application.Messages;
using Questao5.Domain;
using Questao5.Infrastructure.Database.QueryStore;
using Questao5.Infrastructure.Services.Controllers.ResponseModels;

namespace Questao5.Infrastructure.Services.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MovimentoController : ControllerBase
{
    private readonly IMediatorHandler _mediatorHandler;
    private readonly IMovimentoQueryStore _movimentoQueryStore;
    private readonly DomainNotificationHandler _notifications;
    private readonly IContaCorrenteQueryStore _contaCorrenteQueryStore;

    public MovimentoController(IMediatorHandler mediatorHandler, IMovimentoQueryStore movimentoQueryStore, INotificationHandler<DomainNotification> notifications, IContaCorrenteQueryStore contaCorrenteQueryStore)
    {
        _mediatorHandler = mediatorHandler;
        _movimentoQueryStore = movimentoQueryStore;
        _notifications = (DomainNotificationHandler)notifications;
        _contaCorrenteQueryStore = contaCorrenteQueryStore;
    }

    [HttpPost("realizar-movimentacao")]
    public async Task<IActionResult> RealizarMovimentacao(MovimentoRequestModel movimentoRequestModel)
    {    
        var command = new RealizarMovimentacaoCommand(movimentoRequestModel.ChaveIdempotencia, movimentoRequestModel.NumeroConta, movimentoRequestModel.TipoMovimento, movimentoRequestModel.Valor);

        await _mediatorHandler.EnviarComando(command);

        if (OperacaoValida())
        {
            return Ok();
        }

        return BadRequest(ObterMensagensErro());
    }

    [HttpGet("saldo")]
    public async Task<IActionResult> ObterSaldo(int numeroConta)
    {
        var conta = await _contaCorrenteQueryStore.ObterPeloNumero(numeroConta);

        if (conta is null)
        {
            await _mediatorHandler.PublicarNotificacao(new DomainNotification("INVALID_ACCOUNT", $"A conta informada não existe"));
        }
        else if (!conta.Ativo)
        {
            await _mediatorHandler.PublicarNotificacao(new DomainNotification("INACTIVE_ACCOUNT", $"A conta informada está inativa"));
        }

        var saldo = await _movimentoQueryStore.ObterSaldo(numeroConta);

        if (OperacaoValida())
        {
            return Ok(new SaldoResponseModel(conta.Numero, conta.Nome, saldo));
        }

        return BadRequest(ObterMensagensErro());
    }


    protected bool OperacaoValida()
    {

        return !_notifications.TemNotificacao();
    }

    protected IEnumerable<string> ObterMensagensErro()
    {
        return _notifications.ObterNotificacoes().Select(c => c.Value).ToList();
    }
}
