using Domain.Core.Interfaces;
using Domain.Core.Notifications;
using FluentValidation.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Core.Handlers
{
    public abstract class EventHandler : DomainHandler
    {

        #region Variables

        #endregion

        #region Properties

        #endregion

        #region Constructors

        protected EventHandler(IUnitOfWork uow,
                               IMediatorHandler mediator,
                               INotificationHandler<DomainNotification> notifications)
            : base(uow, mediator, notifications)
        {
        }

        #endregion

        #region Methods


        #endregion
    }
}
