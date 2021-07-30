using Domain.Core.Interfaces;
using Domain.Core.Notifications;
using FluentValidation.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Core.Handlers
{
    public class DomainHandler
    {

        #region Variables

        private readonly IUnitOfWork _uow;
        private readonly IMediatorHandler _mediator;
        private readonly DomainNotificationHandler _notifications;

        #endregion

        #region Properties

        public IUnitOfWork UoW => _uow;
        public IMediatorHandler Bus => _mediator;
        private protected DomainNotificationHandler Notifications => _notifications;

        #endregion

        #region Constructors

        protected DomainHandler(IUnitOfWork uow, 
                                IMediatorHandler mediator,
                                INotificationHandler<DomainNotification> notifications)
        {
            _uow = uow;
            _mediator = mediator;
            _notifications = (DomainNotificationHandler)notifications;
        }

        #endregion

        #region Methods

        protected void NotificarErro(string evento, string mensagem)
        {
            _mediator.PublicarEvento(new DomainNotification(evento, mensagem));
        }

        protected void NotificarErro(ValidationResult validationResult)
        {
            foreach (var error in validationResult.Errors)
            {
                _mediator.PublicarEvento(new DomainNotification(error.PropertyName, error.ErrorMessage));
            }
        }        

        #endregion

    }
}
