using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.RegularExpressions;

namespace Domain.Core.ValueObject
{
    public class CPF : ValueObject<CPF>
    {

        #region Variables

        private long _cpf;
        private DateTime? _dataEmissao;

        #endregion

        #region Properties

        public long Numero { get => _cpf; private set => _cpf = value; }
        public string Formatado { get => _cpf.ToString(@"000\.000\.000\-00"); }

        public DateTime? DataEmissao { get => _dataEmissao; private set => _dataEmissao = value; }

        #endregion

        #region Constructors

        // EF Constructor
        private CPF() { }

        public CPF(long cpf, DateTime? dataEmissao = null)
        {
            _cpf = cpf;
            _dataEmissao = dataEmissao;
        }

        public CPF(string cpf, DateTime? dataEmissao = null)
        {
            var strCPF = String.Join("", Regex.Split(cpf, @"[^\d]"));
            _cpf = long.TryParse(strCPF, out long cpfConverted) ? cpfConverted : default;
            _dataEmissao = dataEmissao;
        }

        #endregion

        #region Methods

        public override bool EhValido()
        {
            string numero = _cpf.ToString(@"00000000000");

            if (numero.Length > 11)
                return false;

            while (numero.Length != 11)
                numero = '0' + numero;

            var igual = true;
            for (var i = 1; i < 11 && igual; i++)
                if (numero[i] != numero[0])
                    igual = false;

            if (igual || numero == "12345678909")
                return false;

            var numeros = new int[11];

            for (var i = 0; i < 11; i++)
                numeros[i] = int.Parse(numero[i].ToString());

            var soma = 0;
            for (var i = 0; i < 9; i++)
                soma += (10 - i) * numeros[i];

            var resultado = soma % 11;

            if (resultado == 1 || resultado == 0)
            {
                if (numeros[9] != 0)
                    return false;
            }
            else if (numeros[9] != 11 - resultado)
                return false;

            soma = 0;
            for (var i = 0; i < 10; i++)
                soma += (11 - i) * numeros[i];

            resultado = soma % 11;

            if (resultado == 1 || resultado == 0)
            {
                if (numeros[10] != 0)
                    return false;
            }
            else if (numeros[10] != 11 - resultado)
                return false;

            return true;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            // Using a yield return statement to return each element one at a time
            yield return Numero;
            yield return Formatado;
            yield return DataEmissao;

        }

        #endregion

    }
}
