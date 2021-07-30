using System;

namespace Domain.Core.Events
{
    public abstract class Event : Message
    {

        #region Variables

        private DateTime _timestamp;

        #endregion

        #region Properties

        public DateTime Timestamp { get => _timestamp; private set => _timestamp = value; }

        #endregion

        #region Constructors

        protected Event()
        {
            _timestamp = DateTime.Now;
        }

        #endregion

    }
}
