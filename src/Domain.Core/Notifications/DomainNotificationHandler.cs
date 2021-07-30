using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Domain.Core.Notifications
{
    public class DomainNotificationHandler : INotificationHandler<DomainNotification>
    {

        #region Variables

        private List<DomainNotification> _notifications;
        private readonly ILogger<DomainNotification> _logger;

        #endregion

        #region Constructors

        public DomainNotificationHandler(ILogger<DomainNotification> logger)
        {
            _notifications = new List<DomainNotification>();
            _logger = logger;
        }

        #endregion

        #region Methods

        public virtual bool HasNotifications()
        {
            return _notifications.Any();
        }

        public virtual List<DomainNotification> GetNotifications()
        {
            return _notifications;
        }

        public Task Handle(DomainNotification notification, CancellationToken cancellationToken)
        {

            switch (notification.Key)
            {
                case "SENTRY":
                    _logger.LogError(notification.Key, notification.Value);
                    break;
                default:
                    _notifications.Add(notification);
                    break;
            }

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Erro: {notification.Key} - {notification.Value}");

            return Task.FromResult(notification);
        }


        public void Dispose()
        {
            _notifications = new List<DomainNotification>();
        }

        #endregion

    }
}
