using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Application.Core.ViewModels
{
    /// <summary>
    /// Adiciona os campos de paginação (Número da página consultada, total de registros por página e a ordenação como string)
    /// </summary>
    public abstract class PaginarConsultaViewModel
    {
        public int paginaAtual { get; set; }
        public int registrosPorPagina { get; set; }
        public string filtro { get; set; }
        public string ordenacao { get; set; }

        public PaginarConsultaViewModel()
        {
            paginaAtual = 1;
            registrosPorPagina = 15;
        }
    }
}
