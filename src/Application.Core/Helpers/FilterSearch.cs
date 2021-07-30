using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Application.Core.Helpers
{
    public static class FilterSearch
    {
        public static T Filter<T>(this T entity, string filtro, IDictionary<string, string> dicOrinDest = null)
        {
            // Não possui filtro na pesquisa
            if (string.IsNullOrWhiteSpace(filtro))
                return entity;

            var sortExpressions = new List<Tuple<string, string>>();
            string[] strCampo = filtro.Split(';').Where(str => !string.IsNullOrWhiteSpace(str)).ToArray();

            #region Verificar necessidade de realizar um De/Para nos nomes das propriedades
            if (dicOrinDest != null)
            {
                for (int i = 0; i < strCampo.Count(); i++)
                {
                    var strToConvert = strCampo[i].Split("=")?.First();

                    if (strToConvert != null && dicOrinDest.ContainsKey(strToConvert))
                    {
                        strCampo[i] = strCampo[i].Replace(strToConvert, dicOrinDest[strToConvert]);
                    }
                }

                foreach (var paramOrder in strCampo)
                {
                    if (paramOrder != null)
                    {
                        var columnDePara = paramOrder.Split("=")?.First();
                        var strValue = paramOrder.Split("=")?.Last();

                        Type myType = entity.GetType();
                        PropertyInfo pinfo = myType.GetProperty(columnDePara.ToString());
                        pinfo.SetValue(entity, strValue, null);
                    }
                }
            }

            #endregion

            return entity;
        }

    }
}
