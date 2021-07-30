using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Domain.Core.ValueObject
{
    public class PIS : ValueObject<PIS>
    {

        #region Variables

        private long _pis;

        #endregion

        #region Properties

        public long Numero { get => _pis; private set => _pis = value; }
        public string Formatado { get => _pis.ToString(@"000\.00000\.00\-0"); }

        #endregion

        #region Constructors

        // EF Constructor
        private PIS() { }

        public PIS(long pis)
        {
            _pis = pis;
        }

        public PIS(string pis)
        {
            var strPIS = String.Join("", Regex.Split(pis, @"[^\d]"));
            _pis = long.TryParse(strPIS, out long pisConverted) ? pisConverted : default;
        }

        #endregion

        #region Methods

        public override bool EhValido()
        {
            int[] multiplicador = new int[10] { 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int soma;
            int resto;
            string pis = Formatado;

            if (pis.Trim().Length != 11)
                return false;

            pis = pis.Trim();
            pis = pis.Replace("-", "").Replace(".", "").PadLeft(11, '0');

            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += int.Parse(pis[i].ToString()) * multiplicador[i];

            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            return pis.EndsWith(resto.ToString());
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