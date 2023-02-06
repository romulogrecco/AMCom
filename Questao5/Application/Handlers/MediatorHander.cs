﻿using MediatR;
using Questao5.Application.Messages;

namespace Questao5.Application.Handlers;

public class MediatorHandler : IMediatorHandler
{
    private readonly IMediator _mediator;


    public MediatorHandler(IMediator mediator)
    {
        _mediator = mediator;

    }

    public async Task<bool> EnviarComando<T>(T comando) where T : Command
    {
        return await _mediator.Send(comando);
    }

    public async Task PublicarNotificacao<T>(T notificacao) where T : DomainNotification
    {
        await _mediator.Publish(notificacao);
    }
}