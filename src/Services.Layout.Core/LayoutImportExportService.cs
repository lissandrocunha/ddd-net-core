using Services.Layout.Core.Interfaces;
using Services.Layout.Core.Models;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Services.Layout.Core
{
    public class LayoutImportExportService : ILayoutImportExports
    {

        #region Variables


        #endregion

        #region Constructors

        public LayoutImportExportService()
        {
        }

        #endregion

        #region Methods

        public Models.Layout ObterLayout(string caminhoArquivo)
        {

            string jsonLayoutPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), caminhoArquivo);
            var jsonFile = File.ReadAllText(jsonLayoutPath);
            JObject jsonData = JObject.Parse(jsonFile);

            ICollection<Linha> linhas = new List<Linha>();

            if (jsonData != null && jsonData.ContainsKey("linhas"))
            {
                foreach (JObject linha in jsonData["linhas"])
                {
                    try
                    {

                        var novaLinha = ObterLinhaDoLayout(linha);

                        if (novaLinha != null)
                        {
                            linhas.Add(novaLinha);
                        }
                    }
                    catch (Exception ex)
                    {
                        continue;
                    }
                }
            }

            var layout = Models.Layout.Factory.Novo(linhas);

            return layout;
        }

        private Linha ObterLinhaDoLayout(JObject linha)
        {
            ICollection<Campo> campos = new List<Campo>();

            try
            {
                if (!linha.ContainsKey("id") ||
                    !linha.ContainsKey("campos") ||
                    !linha.ContainsKey("separador"))
                {
                    return null;
                }

                foreach (JObject campo in linha["campos"])
                {
                    if (!campo.ContainsKey("campo") ||
                        !campo.ContainsKey("posicao") ||
                        !campo.ContainsKey("tamanho") ||
                        !campo.ContainsKey("tipo"))
                    {
                        continue;
                    }


                    if (string.IsNullOrWhiteSpace(campo["campo"].ToString())) continue;

                    campos.Add(Campo.Factory.Novo(campo["campo"].ToString(),
                                                  int.TryParse(campo["posicao"].ToString(), out int intPosInicial) ? (int?)intPosInicial : null,
                                                  int.TryParse(campo["tamanho"].ToString(), out int intTamanho) ? (int?)intTamanho : null,
                                                  campo["tipo"].ToString()));
                }

                if (!campos.Where(x => !string.IsNullOrWhiteSpace(x.Nome)).Any()) return null;

                return Linha.Factory.Nova(linha["id"].ToString(),
                                          campos,
                                          linha["separador"].ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IDictionary<string, object> ExtrairCamposToDic<TEntity>(JObject linha)
        {
            IDictionary<string, object> campos = new Dictionary<string, object>();


            foreach (JProperty propriedade in linha.Properties())
            {

                if (typeof(TEntity).GetProperties().Any(p => p.Name == propriedade.Name))
                {
                    campos.Add(propriedade.Name, propriedade.Value);
                }
            }

            return campos;
        }

        public JObject ImportarLayout(string caminhoArquivo, bool layoutFixo, byte[] arquivo, Encoding codificacao = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(caminhoArquivo) ||
                        arquivo == null)
                {
                    return null;
                }

                var layout = ObterLayout(caminhoArquivo);

                if (layout == null) return null;

                if (codificacao == null) codificacao = Encoding.Default;

                string arquivoTexto = codificacao.GetString(arquivo);

                if (string.IsNullOrWhiteSpace(arquivoTexto)) return null;

                IEnumerable<string> arquivoLinhas = arquivoTexto.Split(Environment.NewLine);

                if (arquivoLinhas == null || arquivoLinhas.Count() <= 0) return null;

                if (layoutFixo)
                    return ImportarLayoutFixo(layout, arquivoLinhas);

                return ImportarLayoutDinamico(layout, arquivoLinhas);

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private JObject ImportarLayoutDinamico(Models.Layout layout, IEnumerable<string> arquivoLinhas)
        {
            return null;
        }

        private JObject ImportarLayoutFixo(Models.Layout layout, IEnumerable<string> arquivoLinhas)
        {
            if (layout == null || layout.Linhas.Count == 0) return null;

            dynamic linhasImportadas = new JArray();

            int numeroDaLinha = 1;
            string[] identificacaoDaLinha = layout.Linhas?
                                                  .Select(x => x.Identificacao)?
                                                  .Where(x => x != null)
                                                  .ToArray();
            int tamanhoIdentificacaoLinha = identificacaoDaLinha != null ?
                                            identificacaoDaLinha.OrderByDescending(o => o.Length)
                                                                .FirstOrDefault()
                                                                .Length : 0;

            foreach (var linha in arquivoLinhas)
            {
                if (string.IsNullOrWhiteSpace(linha)) continue;

                dynamic jsonLinha = new JObject();
                string linhaID = linha.Substring(0, tamanhoIdentificacaoLinha);

                if (identificacaoDaLinha != null &&
                    identificacaoDaLinha.Count() > 0 &&
                    identificacaoDaLinha.Contains(linhaID))
                {
                    var linhaLayoutIdentificada = layout.Linhas
                                                        .Where(x => x.Identificacao == linha.Substring(0, tamanhoIdentificacaoLinha))
                                                        .FirstOrDefault();

                    if (linhaLayoutIdentificada == null) continue;

                    jsonLinha = ConverterLinhaFixaEmJObject(linha,
                                                           linhaLayoutIdentificada);
                }

                if (jsonLinha != null)
                {
                    jsonLinha.id = linhaID;
                    linhasImportadas.Add(jsonLinha);
                }

                numeroDaLinha++;
            }

            var importacao = new JObject(new JProperty("linhas", linhasImportadas));

            return importacao;
        }

        private JObject ConverterLinhaFixaEmJObject(string linha,
                                                    Linha linhaLayout)
        {
            if (!linhaLayout.Campo.Where(x => !string.IsNullOrWhiteSpace(x.Nome)).Any()) return null;

            IDictionary<string, object> linhaImportada = new Dictionary<string, object>();

            foreach (var campo in linhaLayout.Campo)
            {
                if (string.IsNullOrWhiteSpace(campo.Nome)) continue;

                int posicaoInicial = int.TryParse(campo.PosicaoInicial.ToString(), out int intPosIni) ? intPosIni : 0;
                int tamanho = int.TryParse(campo.Tamanho.ToString(), out int intTamanho) ? intTamanho : 0;
                string valor = null;

                if (tamanho == 0)
                {
                    valor = linha.Substring(posicaoInicial)?
                                 .Trim();
                }
                else
                {
                    valor = linha.Substring(posicaoInicial,
                                            linha.Length - posicaoInicial - tamanho >= 0 ?
                                            tamanho :
                                            linha.Length - posicaoInicial)?
                                 .Trim();
                }

                switch (campo.Tipo)
                {
                    case "int":
                        if (int.TryParse(valor, out int intConvert))
                        {
                            linhaImportada.Add(campo.Nome, intConvert);
                        }
                        continue;
                    case "long":
                        if (long.TryParse(valor, out long longConvert))
                        {
                            linhaImportada.Add(campo.Nome, longConvert);
                        }
                        continue;
                    case "decimal":
                    case "float":
                        if (decimal.TryParse(valor, out decimal decimalConvert))
                        {
                            linhaImportada.Add(campo.Nome, decimalConvert);
                        }
                        continue;
                    case "date":
                        if (DateTime.TryParse(valor, out DateTime dateConvert))
                        {
                            linhaImportada.Add(campo.Nome, dateConvert);
                        }
                        continue;
                    case "bool":
                    case "boolean":
                        if (bool.TryParse(valor, out bool boolConvert))
                        {
                            linhaImportada.Add(campo.Nome, boolConvert);
                        }
                        continue;
                }

                linhaImportada.Add(campo.Nome, valor);
            }

            return JObject.FromObject(linhaImportada);
        }

        #endregion

    }
}
