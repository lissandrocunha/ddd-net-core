using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Domain.Core.ValueObject
{
    public class CEP : ValueObject<CEP>
    {

        #region Variables

        private long _cep;

        #endregion

        #region Properties

        public long Numero { get => _cep; private set => _cep = value; }
        public string Formatado { get => _cep.ToString(@"00\.000\-000"); }        

        #endregion

        #region Constructors

        // EF Constructor
        private CEP() { }

        public CEP(long cep)
        {
            _cep = cep;
        }

        public CEP(string cep)
        {
            var strCEP = String.Join("", Regex.Split(cep, @"[^\d]"));
            _cep = long.TryParse(strCEP, out long cepConverted) ? cepConverted : default;
        }

        #endregion

        #region Methods

        public override bool EhValido()
        {
            if (string.IsNullOrEmpty(Formatado)) return false;
            if (_cep.ToString() == "00000000") return false;
            if (_cep.ToString() == "11111111") return false;
            if (_cep.ToString() == "22222222") return false;
            if (_cep.ToString() == "33333333") return false;
            if (_cep.ToString() == "44444444") return false;
            if (_cep.ToString() == "55555555") return false;
            if (_cep.ToString() == "66666666") return false;
            if (_cep.ToString() == "77777777") return false;
            if (_cep.ToString() == "88888888") return false;
            if (_cep.ToString() == "99999999") return false;
            return true;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            // Using a yield return statement to return each element one at a time
            yield return Numero;
            yield return Formatado;

        }

        #endregion

    }
}
