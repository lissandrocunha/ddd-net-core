using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Reports.Core.Interfaces
{
    public interface IReportHtmlService
    {

        /// <summary>
        /// Cria uma string Html trocando os parametros e as imagens
        /// </summary>
        /// <param name="htmlReport">nome do arquivo html que será usado com template</param>
        /// <param name="parameters">dicionário contendo o parametro que deverá ser substituido no template e o valor do parâmetro</param>
        /// <param name="images">dicinário contendo o nome do parametro que deverá ser trocado pela imagem e o valor é o caminho da imagem que devera ser importada no html</param>
        /// <returns>string contendo o arquivo html</returns>
        string CreateReport(string htmlReport,
                            Dictionary<string, object> parameters,
                            Dictionary<string, object> images);



        /// <summary>
        /// Cria um byte array do arquivo pdf
        /// </summary>
        /// <param name="htmlReport">nome do arquivo html que será usado com template</param>
        /// <param name="parameters">dicionário contendo o parametro que deverá ser substituido no template e o valor do parâmetro</param>
        /// <param name="images">dicinário contendo o nome do parametro que deverá ser trocado pela imagem e o valor é o caminho da imagem que devera ser importada no html</param>
        /// <param name="paperOrientation">orientação do papel do arquivo pdf (Portrait ou Landscape)</param>
        /// <param name="paperSize">tamanho da folha do arquivo pdf (A0-A10, B0-B10, C0-C10, HalfLetter, Letter, JuniorLegal, Legal, Ledger/Tabloid, Custom)</param>
        /// <param name="paperCustomWidth">largura da folha customizada em milímetros (variável paperSize deve estar como Custom)</param>
        /// <param name="paperCustomHeight">altura da folha customizada milímetros (variável paperSize deve estar como Custom)</param>
        /// <param name="paperMarginTop">distância em milimetros da margem do topo da folha (área referente ao cabeçalho)</param>
        /// <param name="paperMarginLeft">distância em milimetros da margem do lado esquerdo da folha</param>
        /// <param name="paperMarginRight">distância em milimetros da margem do lado direito da folha</param>
        /// <param name="paperMarginBotton">distância em milimetros da margem da base da folha (área referente ao rodapé)</param>
        /// <param name="headerSpacing">distância em milimetros entre o cabeçalho e o conteúdo da folha</param>
        /// <param name="footerSpacing">distância em milimetros entre o conteúdo da folha e o rodapé</param>
        /// <param name="encode">tipo de encode que deve ser usado na geração do PDF</param>
        /// <param name="copies">quantidade de cópias que deverá conter no arquivo</param>
        /// <param name="grayScale">gerar o pdf em escala de cinza</param>
        /// <param name="dpiResolution">gerar o PDF na resolução de dpi informada</param>
        /// <param name="smartShrink">Habilite/Desabilita a estratégia de encolhimento inteligente usado pelo WebKit que faz o pixel/dpi </param>
        /// <param name="zoom">fator de zoom da imagem html</param>
        /// <param name="footerInfo">exibir informação de data e hora da geração e quantidade de páginas no rodapé</param>
        /// <returns>Byte Array do arquivo PDF</returns>
        byte[] CreateReportPdf(string htmlReport,
                               Dictionary<string, object> parameters,
                               Dictionary<string, object> images,
                               string paperOrientation = "Portrait",
                               string paperSize = "A4",
                               double paperCustomWidth = 0,
                               double paperCustomHeight = 0,
                               double paperMarginTop = 0,
                               double paperMarginLeft = 0,
                               double paperMarginRight = 0,
                               double paperMarginBotton = 0,
                               double headerSpacing = 0,
                               double footerSpacing = 0,
                               string encode = null,
                               int copies = 1,
                               bool grayScale = false,
                               int dpiResolution = 0,
                               bool smartShrink = true,
                               double zoom = 1,
                               bool footerInfo = false);

        /// <summary>
        /// Extrai os parametros existentes do HTML
        /// </summary>
        /// <param name="htmlReportPath">Caminho do arquivo html</param>
        /// <param name="idParameterStart">Identificação do começo do parâmetro</param>
        /// <param name="idParameterEnd">Identificação do termino do parâmetro</param>
        /// <returns>Dicionário de Parâmentros sem valor preenchido</returns>
        Dictionary<string, object> ExtractParametersFromHtml(string htmlReportPath,
                                                             string idParameterStart,
                                                             string idParameterEnd);

        /// <summary>
        /// Extrai os parametros existentes do HTML e preenche com o valor das propriedades primitivas da classe do Objeto
        /// </summary>
        /// <param name="htmlReportPath">Caminho do arquivo html</param>
        /// <param name="idParameterStart">Identificação do começo do parâmetro</param>
        /// <param name="idParameterEnd">Identificação do termino do parâmetro</param>
        /// <param name="objClass">Classe que possui as propridades com o mesmo nome dos parâmetros</param>
        /// <returns>Dicionário de Parâmentros com valor preenchido</returns>
        Dictionary<string, object> ExtractParametersFromHtml(string htmlReportPath,
                                                             string idParameterStart,
                                                             string idParameterEnd,
                                                             object objClass);

        /// <summary>
        /// Preenche os parâmetros com o valor da propriedade de nome igual ao parâmetro
        /// </summary>
        /// <param name="parameters">Dicionário de Parâmetro a ser preenchido</param>
        /// <param name="idParameterStart">Identificação do começo do parâmetro</param>
        /// <param name="idParameterEnd">Identificação do termino do parâmetro</param>
        /// <param name="objClass">Classe que possui as propridades com o mesmo nome dos parâmetros</param>
        void FillParameterValueFromObject(Dictionary<string, object> parameters,
                                          string idParameterStart,
                                          string idParameterEnd,
                                          object objClass);


        /// <summary>
        /// Gerar um QRCode do texto passado
        /// </summary>
        /// <param name="encodeText">texto que deverá ser codificado em QR Code</param>
        /// <param name="qrCodeOutputFormat">0 = Pixel Image, 1 = Svg Image</param>
        /// <param name="width">comprimento da imagem</param>
        /// <param name="height">altura da imagem</param>
        /// <returns>Array de Bytes contendo o QR Code</returns>
        byte[] GenerateQRCode(string encodeText,
                              int qrCodeOutputFormat = 0,
                              int width = 100,
                              int height = 100);

    }
}
