using Domain.Core.Commands;
using Domain.Core.Events;
using Domain.Core.Interfaces;
using FluentValidation.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Domain.Core.Handlers
{
    public class MediatorHandler : IMediatorHandler
    {

        #region Variables

        private readonly IMediator _mediator;
        private readonly IEventStore _eventStore;

        #endregion

        #region Constructors

        public MediatorHandler(IMediator mediator,
                               IEventStore eventStore)
        {
            _mediator = mediator;
            _eventStore = eventStore;
        }

        #endregion

        #region Methods        

        public async Task<TResponse> EnviarComando<TResponse>(IRequest<TResponse> command, CancellationToken cancellationToken = default)
        {
            return await _mediator.Send(command, cancellationToken);
        }

        public async Task EnviarComando<T>(T command, CancellationToken cancellationToken = default) where T : Command
        {
            await _mediator.Send(command, cancellationToken);
        }

        public async Task PublicarEvento<T>(T evento, CancellationToken cancellationToken = default) where T : Event
        {
            if (!evento.MessageType.Equals("DomainNotification"))
                _eventStore?.SaveEvent(evento);

            await Publicar(evento, cancellationToken);
        }

        private async Task Publicar<T>(T message, CancellationToken cancellationToken = default) where T : Message
        {
            await _mediator.Publish(message, cancellationToken);
        }

        #endregion

    }
}
