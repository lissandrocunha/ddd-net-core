using System;

namespace Domain.Core.Events
{
    public class StoredEvent : Event
    {

        #region Variables

        private Guid _id;
        private string _data;
        private string _user;

        #endregion

        #region Properties

        public Guid Id { get => _id; private set => _id = value; }
        public string Data { get => _data; private set => _data = value; }
        public string User { get => _user; private set => _user = value; }

        #endregion

        #region Constructors

        // EF Constructor
        protected StoredEvent() { }

        public StoredEvent(Event evento, string data, string user)
        {
            Id = Guid.NewGuid();
            AggregateId = evento.AggregateId;
            MessageType = evento.MessageType;
            Data = data;
            User = user;
        }

        #endregion

    }
}
