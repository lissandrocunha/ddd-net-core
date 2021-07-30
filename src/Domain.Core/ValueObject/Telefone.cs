using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Domain.Core.ValueObject
{
    public class Telefone : ValueObject<Telefone>
    {

        #region Variables

        private long? _telefone = null;

        #endregion

        #region Properties

        public int? DDD
        {
            get
            {
                var stringValue = _telefone.ToString();
                if (stringValue.Length <= 8 || stringValue.Length == 9) return null;
                return int.Parse(stringValue.Substring(0, 2));
            }
        }
        public long? Numero
        {
            get
            {
                if (_telefone == null) return null;
                var stringValue = _telefone.ToString();
                int dddLenght = DDD == null ? 0 : DDD.Value.ToString().Length;
                return int.Parse(stringValue.Substring(dddLenght, stringValue.Length - dddLenght));
            }
        }

        public string Formatado
        {
            get
            {
                var stringValue = _telefone.ToString();
                if (string.IsNullOrEmpty(stringValue)) return null;

                if (stringValue.Length == 8) return _telefone?.ToString(@"0000-0000");
                if (stringValue.Length == 9) return _telefone?.ToString(@"00000-0000");
                if (stringValue.Length == 10) return _telefone?.ToString(@"(00) 0000-0000");
                if (stringValue.Length == 11) return _telefone?.ToString(@"(00) 00000-0000");

                return null;
            }
        }

        #endregion

        #region Constructors

        // EF Constructor
        private Telefone() { }

        public Telefone(long telefone)
        {
            _telefone = telefone;
        }

        public Telefone(string telefone)
        {
            if (string.IsNullOrWhiteSpace(telefone)) return;

            var strTelefone = String.Join("", Regex.Split(telefone, @"[^\d]"));
            _telefone = long.TryParse(strTelefone, out long telefoneConverted) ? telefoneConverted : default;
        }

        #endregion

        #region Methods

        public override bool EhValido()
        {
            var stringValue = _telefone.ToString();
            if (string.IsNullOrEmpty(stringValue)) return false;
            return stringValue.Length == 8  // Residencial
                || stringValue.Length == 9  // Celular
                || stringValue.Length == 10 // Residencial com DDD
                || stringValue.Length == 11;// Celular com DDD
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            throw new NotImplementedException();
        }

        #endregion

    }
}
