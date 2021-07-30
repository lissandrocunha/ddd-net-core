using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Layout.Core.Models
{
    public class Layout
    {

        #region Variables

        private ICollection<Linha> _linhas;

        #endregion

        #region Properties

        internal ICollection<Linha> Linhas { get => _linhas; }        

        #endregion

        #region Constructors

        private Layout() { }

        #endregion

        #region Factories

        public static class Factory
        {

            public static Layout Novo(ICollection<Linha> linhas, bool layoutFixo = true)
            {
                var layout = new Layout()
                {
                    _linhas = linhas                    
                };

                return layout;
            }

        }

        #endregion

    }
}
