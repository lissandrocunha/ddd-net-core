using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Layout.Core.Interfaces
{
    public interface ILayoutImportExports
    {

        Models.Layout ObterLayout(string caminhoArquivo);        
        JObject ImportarLayout(string caminhoArquivo, bool layoutFixo, byte[] arquivo, Encoding codificacao = null);
        IDictionary<string, object> ExtrairCamposToDic<TEntity>(JObject linha);

    }
}
