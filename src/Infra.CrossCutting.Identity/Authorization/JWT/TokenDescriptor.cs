using System;
using System.Collections.Generic;
using System.Text;

namespace Infra.CrossCutting.Identity.Authorization.JWT
{
    public class TokenDescriptor
    {

        #region Variables

        private string _authority;
        private string _audience;
        private string _issuer;
        private int _minutesValid;
        private int _finalExpiration;
        private int _clockSkew;

        #endregion

        #region Properties

        public string Authority { get => _authority; set => _authority = value; }
        public string Audience { get => _audience; set => _audience = value; }
        public string Issuer { get => _issuer; set => _issuer = value; }
        public int MinutesValid { get => _minutesValid; set => _minutesValid = value; }
        public int FinalExpiration { get => _finalExpiration; set => _finalExpiration = value; }
        public int ClockSkew { get => _clockSkew; set => _clockSkew = value; }

        #endregion

        #region Constructors

        #endregion

        #region Methods

        #endregion

    }
}
