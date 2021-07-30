using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain.Core.ValueObject
{
    public class Name : ValueObject<Name>
    {

        #region Variables

        private string _prefixo;
        private string _nome;
        private string _nomeMeio;
        private string _sobrenome;
        private string _nomeCompleto;
        private char[] _iniciais;

        #endregion

        #region Properties

        public string Nome { get => _nome; private set => _nome = value; }
        public string NomeMeio { get => _nomeMeio; private set => _nomeMeio = value; }
        public string Sobrenome { get => _sobrenome; private set => _sobrenome = value; }
        public string Iniciais { get => _iniciais == null || _iniciais.Length == 0 ? null : string.Join(' ', _iniciais); }
        public string NomeCompleto { get => _nomeCompleto; }

        #endregion

        #region Constructors

        // EF Constructor
        private Name() { }

        public Name(string nomeCompleto, string prefixo = null)
        {
            if (string.IsNullOrWhiteSpace(nomeCompleto))
                return;

            string[] strNome = nomeCompleto.Split(" ");

            _prefixo = prefixo;
            _nome = strNome.First();
            _nomeMeio = strNome.Count() == 2 ? null : string.Join(" ", strNome.Skip(1).ToArray().SkipLast(1).ToArray());
            _sobrenome = strNome.Last();
            _nomeCompleto = ObtemNomeCompleto();
            _iniciais = ObtemIniciais();

        }

        public Name(string primeiroNome, string nomeMeio, string sobrenome, string prefixo = null)
        {
            _prefixo = prefixo;
            _nome = primeiroNome;
            _nomeMeio = nomeMeio;
            _sobrenome = sobrenome;
            _nomeCompleto = ObtemNomeCompleto();
            _iniciais = ObtemIniciais();
        }

        #endregion

        #region Methods

        private string ObtemNomeCompleto()
        {
            string nomeCompleto = "";
            nomeCompleto += string.IsNullOrWhiteSpace(_nome) ? "" : _nome.Trim();
            nomeCompleto += string.IsNullOrWhiteSpace(_nomeMeio) ? "" : _nomeCompleto + " " + _nomeMeio.Trim();
            nomeCompleto += string.IsNullOrWhiteSpace(_sobrenome) ? "" : _nomeCompleto + " " + _sobrenome.Trim();

            return nomeCompleto;
        }


        private char[] ObtemIniciais()
        {
            return _nomeCompleto.Split((char[])null, StringSplitOptions.RemoveEmptyEntries)
                .Select(w => char.ToUpper(w[0]))
                .ToArray();
        }

        public override bool EhValido()
        {
            return !string.IsNullOrEmpty(NomeCompleto);
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Nome;
            yield return NomeMeio;
            yield return Sobrenome;
            yield return Iniciais;
            yield return NomeCompleto;
        }

        public override string ToString()
        {
            string nomeCompleto = string.Empty;

            nomeCompleto = (string.IsNullOrWhiteSpace(_prefixo) ? "" : _prefixo + " ") + _nomeCompleto;

            return nomeCompleto;
        }

        #endregion

    }
}
