using Domain.Core.Interfaces;
using Domain.Core.Notifications;
using FluentValidation.Results;
using MediatR;
using System;

namespace Domain.Core.Handlers
{
    public abstract class CommandHandler : DomainHandler
    {

        #region Variables


        #endregion

        #region Properties


        #endregion

        #region Constructors

        protected CommandHandler(IUnitOfWork uow,
                                 IMediatorHandler mediator,
                                 INotificationHandler<DomainNotification> notifications)
            : base(uow, mediator, notifications)

        {
        }

        #endregion

        #region Methods

        protected bool Commit()
        {
            if (Notifications.HasNotifications()) return false;

            if (UoW.Commit()) return true;

            Exception lastException = UoW.LastException;
            var exceptionMessage = lastException.InnerException != null ? lastException.InnerException.Message : lastException.Message;

            NotificarErro("SENTRY", exceptionMessage);
            NotificarErro("Commit", "Ocorreu um erro ao salvar os dados no banco");
            return false;
        }

        #endregion

    }
}
