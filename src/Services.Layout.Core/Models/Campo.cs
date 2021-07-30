using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Layout.Core.Models
{
    public class Campo
    {

        #region Variables

        private string _nome;
        private int? _posicaoInicial;
        private int? _tamanho;
        private string _tipo;

        #endregion

        #region Properties

        public string Nome { get => _nome; }
        public int? PosicaoInicial { get => _posicaoInicial; }
        public int? Tamanho { get => _tamanho; }
        public string Tipo { get => _tipo; }

        #endregion

        #region Constructors

        private Campo() { }

        #endregion

        #region Factories

        public static class Factory
        {
            public static Campo Novo(string nome,
                                     int? posicaoInicial,
                                     int? tamanho,
                                     string tipo)
            {
                var campo = new Campo()
                {
                    _nome = nome,
                    _posicaoInicial = posicaoInicial,
                    _tamanho = tamanho,
                    _tipo = tipo
                };

                return campo;
            }

        }

        #endregion

    }
}
