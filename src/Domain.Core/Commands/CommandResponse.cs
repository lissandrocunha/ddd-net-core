using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Core.Commands
{
    public class CommandResponse
    {

        #region Variables

        private static CommandResponse _ok = new CommandResponse(true);
        private static CommandResponse _fail = new CommandResponse(false);
        private bool _success;

        #endregion

        #region Properties

        public static CommandResponse Ok { get => _ok; private set => _ok = value; }
        public static CommandResponse Fail { get => _fail; set => _fail = value; }

        public bool Success { get => _success; private set => _success = value; }        

        #endregion

        #region Constructors

        public CommandResponse(bool success = false)
        {
            _success = success;
        }

        #endregion

        #region Methods

        #endregion

        #region Factories

        #endregion



    }
}
