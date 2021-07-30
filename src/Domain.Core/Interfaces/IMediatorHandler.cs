using Domain.Core.Commands;
using Domain.Core.Events;
using FluentValidation.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Domain.Core.Interfaces
{
    public interface IMediatorHandler
    {

        Task<TResponse> EnviarComando<TResponse>(IRequest<TResponse> comando, CancellationToken cancellationToken = default(CancellationToken));
        Task EnviarComando<T>(T command, CancellationToken cancellationToken = default(CancellationToken)) where T : Command;
        Task PublicarEvento<T>(T evento, CancellationToken cancellationToken = default(CancellationToken)) where T : Event;
        
    }
}
