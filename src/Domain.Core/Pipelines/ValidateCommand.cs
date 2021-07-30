using Domain.Core.Commands;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Domain.Core.Pipelines
{
    public class ValidateCommand<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
           where TResponse : CommandResponse
    {
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            //if (request is Domain.Validatable validatable)
            //{
            //    validatable.Validate();
            //    if (validatable.Invalid)
            //    {
            //        Domain.Result validations = new Domain.Result();
            //        foreach (Flunt.Notifications.Notification notification in validatable.Notifications)
            //            validations.AddValidation(notification.Message);

            //        var response = validations as TResponse;
            //        return response;
            //    }
            //}

            TResponse result = await next();
            return result;
        }
    }
}
