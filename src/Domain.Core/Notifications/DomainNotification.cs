using Domain.Core.Events;
using System;

namespace Domain.Core.Notifications
{
    public class DomainNotification : Event
    {

        #region Variables

        private Guid _domainNotificationId;
        private string _key; // Nome do evento
        private string _value; // Mensagem do evento
        private int _version; // versao da notificacao de dominio

        #endregion

        #region Properties

        public Guid DomainNotificationId { get => _domainNotificationId; private set => _domainNotificationId = value; }
        public string Key { get => _key; private set => _key = value; }
        public string Value { get => _value; private set => _value = value; }
        public int Version { get => _version; private set => _version = value; }

        #endregion

        #region Constructors

        public DomainNotification(string key,
                                  string value)
        {
            _domainNotificationId = Guid.NewGuid();
            _key = key;
            _value = value;
            _version = 1;
        }

        #endregion

    }
}
