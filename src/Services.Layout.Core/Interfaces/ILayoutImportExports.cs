using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Layout.Core.Interfaces
{
    public interface ILayoutImportExports
    {

        Layout ObterLayout(string caminhoArquivo);        
        JObject ImportarLayout(string caminhoArquivo, bool layoutFixo, byte[] arquivo, Encoding codificacao = null);
        IDictionary<string, object> ExtrairCamposToDic<TEntity>(JObject linha);

    }
}
