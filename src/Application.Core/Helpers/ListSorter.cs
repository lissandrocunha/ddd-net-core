using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Application.Core.Helpers
{
    public enum SortMethod
    {
        [Description("OrderBy")]
        OrderBy = 0,

        [Description("OrderByDescending")]
        OrderByDescending = 1,

        [Description("ThenBy")]
        ThenBy = 2,

        [Description("ThenByDescending")]
        ThenByDescending = 3
    }

    public static class ListSorter
    {
        public static IQueryable<T> Ordenation<T>(this IQueryable<T> entity, string ordenation, IDictionary<string, string> dicOrinDest = null)
        {
            // Não precisa ordenar
            if (string.IsNullOrWhiteSpace(ordenation))
                return entity;

            var sortExpressions = new List<Tuple<string, string>>();
            string[] strOrdenations = ordenation.Split(';')
                                                .Where(str => !string.IsNullOrWhiteSpace(str))
                                                .ToArray();
            bool firstExpression = true;

            #region Verificar necessidade de realizar um De/Para nos nomes das propriedades

            if (dicOrinDest != null)
            {
                for (int i = 0; i < strOrdenations.Count(); i++)
                {
                    var strToConvert = strOrdenations[i].Split(":")?.First();
                    if (strToConvert != null
                     && dicOrinDest.ContainsKey(strToConvert))
                    {
                        strOrdenations[i] = strOrdenations[i].Replace(strToConvert, dicOrinDest[strToConvert]);
                    }
                }
            }

            #endregion

            #region Preparar Expressão de Ordenação

            foreach (var paramOrder in strOrdenations)
            {
                string[] strOrder = paramOrder.Split(':');
                string fieldName = strOrder.First().Trim();
                string sortDirection = strOrder.Last() == fieldName || string.IsNullOrWhiteSpace(strOrder.Last()) ? "ASC" : strOrder.Last().Trim();

                sortExpressions.Add(new Tuple<string, string>(fieldName, sortDirection));
            }

            #endregion

            // Não precisa ordenar
            if ((sortExpressions == null) || (sortExpressions.Count <= 0))
                return entity;

            IEnumerable<T> query = from item in entity select item;
            IOrderedEnumerable<T> orderedQuery = null;

            foreach (var sortExpression in sortExpressions)
            {

                Func<T, object> expression = item => item.GetType()
                                .GetProperty(sortExpression.Item1)?
                                .GetValue(item, null);

                if (firstExpression)
                {
                    if (sortExpression.Item2.ToUpper() == "ASC")
                        orderedQuery = query.OrderBy(expression);
                    else
                        orderedQuery = query.OrderByDescending(expression);

                    firstExpression = false;
                }
                else
                {
                    if (sortExpression.Item2.ToUpper() == "ASC")
                        orderedQuery = orderedQuery.ThenBy(expression);
                    else
                        orderedQuery = orderedQuery.ThenByDescending(expression);
                }
            }

            query = orderedQuery;

            return query.AsQueryable();
        }

    }
}