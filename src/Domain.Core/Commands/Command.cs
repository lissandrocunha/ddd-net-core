using Domain.Core.Events;
using FluentValidation.Results;
using System;

namespace Domain.Core.Commands
{
    public abstract class Command : Message
    {

        #region Variables

        private DateTime _timestamp;
        private ValidationResult _validationResult;

        #endregion

        #region Properties

        public DateTime Timestamp { get => _timestamp; private set => _timestamp = value; }
        public ValidationResult ValidationResult { get => _validationResult; set => _validationResult = value; }

        #endregion

        #region Constructors

        public Command()
        {
            _timestamp = DateTime.Now;
            _validationResult = new ValidationResult();
        }

        #endregion

        #region Methods

        public virtual bool EhValido()
        {
            throw new NotImplementedException();
        }

        #endregion

    }
}
