using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Core.ValueObject
{
    public class Endereco : ValueObject<Endereco>
    {

        #region Variables

        private string _logradouro;
        private string _numero;
        private string _complemento;
        private string _bairro;
        private string _cidade;
        private string _estado;
        private string _pais;
        private CEP _cep;

        #endregion

        #region Properties

        public string Logradouro { get => _logradouro; }
        public string LogradouroCompleto { 
            get {
                return _logradouro + ", " + Numero + ( string.IsNullOrWhiteSpace(_complemento) ? "" : ", " + _complemento);
            }
        }
        public string Numero { get => string.IsNullOrWhiteSpace(_numero) ? "S/N" : _numero; }
        public string Complemento { get => _complemento; }
        public string Bairro { get => _bairro; }
        public string Cidade { get => _cidade; }
        public string Estado { get => _estado; }
        public string Pais { get => _pais; }
        public CEP CEP { get => _cep; }

        #endregion

        #region Constructors

        // EF Constructor
        private Endereco() { }

        public Endereco(string logradouro,
                        string numero,
                        string cidade,
                        string estado,
                        string cep,
                        string bairro = null,
                        string complemento = null,
                        string pais = null)
        {
            _logradouro = logradouro;
            _numero = numero;
            _cidade = cidade;
            _complemento = complemento;
            _bairro = bairro;
            _estado = estado;
            _pais = pais;
            _cep = new CEP(cep);
        }

        public Endereco(string endereco)
        {
            if (string.IsNullOrWhiteSpace(endereco)) return;

            string campo = string.Empty;

            foreach (var caracter in endereco.ToCharArray())
            {
                if (caracter == ',')
                {
                    if (string.IsNullOrWhiteSpace(_logradouro))
                    {
                        _logradouro = campo.Trim();
                    }
                }

                campo += caracter;
            }
        }

        #endregion

        #region Methods

        public override string ToString()
        {
            string endereco = _logradouro;
            endereco += ", " + Numero;
            endereco += string.IsNullOrWhiteSpace(_complemento) ? "" : ", " + _complemento;
            endereco += string.IsNullOrWhiteSpace(_bairro) ? "" : " - " + _bairro;
            endereco += string.IsNullOrWhiteSpace(_cidade) ? "" : ", " + _cidade + (string.IsNullOrWhiteSpace(_estado) ? "" : " - " + _estado);
            endereco += (_cep != null && _cep.Numero == default) ? "" : ", " + _cep.Formatado;

            return endereco;
        }

        public override bool EhValido()
        {
            return false;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            // Using a yield return statement to return each element one at a time
            yield return Logradouro;
            yield return LogradouroCompleto;
            yield return Numero;
            yield return Cidade;
            yield return Estado;
            yield return Pais;
            yield return CEP;

        }

        #endregion

        #region Enum

        public enum UnidadeFederacao
        {
            AC, // = "Acre";
            AL, // = "Alagoas";
            AP, // = "Amapá";
            AM, // = "Amazonas";
            BA, // = "Bahia";
            CE, // = "Ceará";
            DF, // = "Distrito Federal";
            ES, // = "Espirito Santo";
            GO, // = "Goiás";
            MA, // = "Maranhão";
            MT, // = "Mato Grosso";
            MS, // = "Mato Grosso do Sul";
            MG, // = "Minas Gerais";
            PA, // = "Pará";
            PB, // = "Paraiba";
            PR, // = "Paraná";
            PE, // = "Pernambuco";
            PI, // = "Piauí";
            RJ, // = "Rio de Janeiro";
            RN, // = "Rio Grande do Norte";
            RS, // = "Rio Grande do Sul";
            RO, // = "Rondônia";
            RR, // = "Roraima";
            SC, // = "Santa Catarina";
            SP, // = "São Paulo";
            SE, // = "Sergipe";
            TO  // = "Tocantis";
        }


        #endregion

    }
}
