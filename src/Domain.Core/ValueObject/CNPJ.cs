using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Domain.Core.ValueObject
{
    public class CNPJ : ValueObject<CNPJ>
    {

        #region Variables

        private long _cnpj;

        #endregion

        #region Properties

        public long Numero { get => _cnpj; private set => _cnpj = value; }
        public string Formatado { get => _cnpj.ToString(@"00\.000\.000\/0000\-00"); }

        #endregion

        #region Constructor

        // EF Constructor
        private CNPJ() { }

        public CNPJ(long cnpj)
        {
            _cnpj = cnpj;
        }

        public CNPJ(long? cnpj)
        {
            if (cnpj != null)
            _cnpj = cnpj.Value;
        }

        public CNPJ(string cnpj)
        {
            var strCNPJ = String.Join("", Regex.Split(cnpj, @"[^\d]"));
            _cnpj = long.TryParse(strCNPJ, out long cnpjConverted) ? cnpjConverted : default;
        }

        #endregion

        #region Methods

        public override bool EhValido()
        {
            int[] multiplicador1 = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int soma;
            int resto;
            string digito;
            string tempCnpj;
            string cnpj = Formatado;

            cnpj = cnpj.Trim();
            cnpj = cnpj.Replace(".", "").Replace("-", "").Replace("/", "");
            if (cnpj.Length != 14)
                return false;

            tempCnpj = cnpj.Substring(0, 12);
            soma = 0;
            for (int i = 0; i < 12; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador1[i];
            resto = (soma % 11);
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            digito = resto.ToString();
            tempCnpj = tempCnpj + digito;
            soma = 0;
            for (int i = 0; i < 13; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador2[i];
            resto = (soma % 11);
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = digito + resto.ToString();

            return cnpj.EndsWith(digito);
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
