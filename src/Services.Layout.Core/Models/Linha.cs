using System.Collections.Generic;

namespace Services.Layout.Core.Models
{
    public class Linha
    {

        #region Variables

        private string _identificacao;
        private ICollection<Campo> _campos;
        private string _separador;

        #endregion

        #region Properties

        public string Identificacao { get => _identificacao; }
        public ICollection<Campo> Campo { get => _campos; }
        public string Separador { get => _separador; }

        #endregion

        #region Constructors

        private Linha() { }

        #endregion

        #region Factories

        public static class Factory
        {
            public static Linha Nova(string identificacao,
                                     ICollection<Campo> campos,
                                     string separador = null)
            {

                var linha = new Linha()
                {
                    _identificacao = identificacao,
                    _campos = campos,
                    _separador = separador
                };

                return linha;
            }

        }

        #endregion

    }
}