using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Core.ValueObject
{
    public class Email : ValueObject<Email>
    {

        #region Variables

        private string _email;

        #endregion

        #region Properties

        public string Usuario
        {
            get
            {
                if (EhValido() == false) return null;

                return _email.Split("@")[0];
            }
        }

        public string Dominio
        {
            get
            {
                if (EhValido() == false) return null;
                return _email.Split("@")[1];
            }
        }

        #endregion

        #region Constructors

        // EF Constructor
        private Email()
        {

        }

        public Email(string value)
        {
            this._email = null;
            if (value.IndexOf('@') > -1)
            {
                this._email = value;
            }
        }

        #endregion

        #region Methods

        public override string ToString()
        {
            return _email;
        }

        public override bool EhValido()
        {
            if (string.IsNullOrEmpty(_email)) return false;

            try
            {
                var addr = new System.Net.Mail.MailAddress(_email);
                return addr.Address == _email;
            }
            catch
            {
                return false;
            }
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            // Using a yield return statement to return each element one at a time
            yield return Usuario;
            yield return Dominio;
        }

        #endregion

    }
}
