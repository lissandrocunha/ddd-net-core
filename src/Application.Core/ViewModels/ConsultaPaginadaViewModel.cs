using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Core.ViewModels
{
    /// <summary>
    /// Classe de retorno com propriedades de Paginação
    /// </summary>
    /// <typeparam name="T">Classe da entidade de Retorno</typeparam>
    public class ConsultaPaginadaViewModel<T> where T : class
    {

        #region Variables

        private ICollection<T> _registros;
        private long? _totalDeRegistros;
        private long? _paginaAtual;
        private long? _registrosPorPagina;

        #endregion

        #region Properties

        public ICollection<T> Registros { get => _registros; }
        public long TotalDeRegistros
        {
            get
            {

                if (_registros == null) return 0;

                if (_totalDeRegistros == null) return _registros.Count;

                return long.TryParse(_totalDeRegistros.ToString(), out long convTotReg) ? convTotReg : _registros.Count;

            }
        }
        public long? PaginaAtual { get => _paginaAtual; }
        public long? RegistrosPorPagina { get => _registrosPorPagina; }

        #endregion

        #region Constructos

        public ConsultaPaginadaViewModel(ICollection<T> registros,
                                         long? totalDeRegistros = null,
                                         long? paginaAtual = null,
                                         long? registrosPorPagina = null)
        {
            _registros = registros;
            _totalDeRegistros = totalDeRegistros;
            _paginaAtual = paginaAtual;
            _registrosPorPagina = registrosPorPagina;
        }

        #endregion

    }
}
