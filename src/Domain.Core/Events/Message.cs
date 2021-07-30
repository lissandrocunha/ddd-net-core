using MediatR;
using System;

namespace Domain.Core.Events
{
    public abstract class Message : INotification
    {

        #region Variables

        private string _messageType;
        private Guid _aggregateId;

        #endregion

        #region Properties

        public string MessageType { get => _messageType; protected set => _messageType = value; }
        public Guid AggregateId { get => _aggregateId; protected set => _aggregateId = value; }

        #endregion

        #region Constructors

        protected Message()
        {
            AggregateId = Guid.NewGuid();
            MessageType = GetType().Name;
        }

        #endregion

    }
}
