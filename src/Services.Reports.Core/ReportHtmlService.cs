using Services.Reports.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using ZXing;
using ZXing.QrCode;
using ZXing.Rendering;
using static ZXing.Rendering.SvgRenderer;

namespace Faepa.ReportService.Core
{
    public class ReportHtmlService : IReportHtmlService
    {

        #region Variables

        //private readonly IHostingEnvironment _hostingEnvironment;

        #endregion

        #region Constructors

        public ReportHtmlService()
        {
            //_hostingEnvironment = hostingEnvironment;
        }

        #endregion

        #region Methods

        public string CreateReport(string htmlReport,
                                   Dictionary<string, object> parameters,
                                   Dictionary<string, object> images)
        {
            string report = null;

            var htmlPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), htmlReport);

            if (!File.Exists(htmlPath)) return report;

            string htmlFile = File.ReadAllText(htmlPath);

            if (string.IsNullOrWhiteSpace(htmlFile)) return report;

            #region Substituindo Parâmetros

            if (parameters != null)
            {
                foreach (var param in parameters)
                {
                    htmlFile = htmlFile.Replace(param.Key, param.Value == null ? "" : param.Value.ToString());
                }
            }

            #endregion

            #region Substituindo Imagens

            if (images != null)
            {
                foreach (var image in images)
                {
                    var imageFile = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), image.Value.ToString());
                    string imageBase64;

                    if (!File.Exists(imageFile)) continue;

                    using (Image img = Image.FromFile(imageFile))
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            img.Save(ms, img.RawFormat);
                            byte[] imgBytes = ms.ToArray();
                            imageBase64 = Convert.ToBase64String(imgBytes);
                        }
                    }

                    string initialContent = "data:image/" + Path.GetExtension(image.Value.ToString()).TrimStart('.') + ";base64,";

                    htmlFile = htmlFile.Replace(image.Key, initialContent + imageBase64);
                }
            }

            #endregion

            report = htmlFile;

            return report;
        }

        public byte[] CreateReportPdf(string htmlReport,
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
                                      bool footerInfo = false)
        {
            byte[] pdf;

            var htmlFile = CreateReport(htmlReport,
                                        parameters,
                                        images);

            var pdfConvert = Pdf.From(htmlFile);

            if (paperSize == "Custom"
             && paperCustomWidth > 0
             && paperCustomHeight > 0)
            {
                var customPaper = new PaperSize(Length.Millimeters(paperCustomWidth),
                                                Length.Millimeters(paperCustomHeight));

                pdfConvert.PaperSize(customPaper);
            }
            else
            {

                Type t = typeof(PaperSize);
                PropertyInfo prop = t.GetProperty(paperSize);
                if (prop != null)
                {
                    pdfConvert.PaperSize(prop.GetValue(this, null) as PaperSize);
                }
                else
                {
                    pdfConvert.PaperSize(PaperSize.A4);
                }
            }

            var paperMaginsConfig = PaperMargins.Custom(Length.Millimeters(paperMarginTop),
                                                        Length.Millimeters(paperMarginBotton),
                                                        Length.Millimeters(paperMarginLeft),
                                                        Length.Millimeters(paperMarginRight));

            pdfConvert.WithMargins(paperMaginsConfig);

            if (paperOrientation == "Landscape")
                pdfConvert.Landscape();
            else
                pdfConvert.Portrait();

            if (!string.IsNullOrWhiteSpace(encode))
                pdfConvert.EncodedWith(encode);

            if (dpiResolution > 0)
                pdfConvert.WithResolution(dpiResolution);

            pdfConvert.WithSmartShrink(smartShrink);

            if (grayScale)
                pdfConvert.GrayScale();

            if (copies > 1)
                pdfConvert.WithCopies(copies);

            pdfConvert.WithZoom(zoom);

            pdfConvert.WithHeaderSpacing(headerSpacing);

            pdfConvert.WithFooterSpacing(footerSpacing);

            pdfConvert.Compressed();

            if (footerInfo)
                pdfConvert.WithFooterCustom("--footer-right \"[page]/[toPage]\"");

            pdf = pdfConvert.Content();

            return pdf;
        }

        public Dictionary<string, object> ExtractParametersFromHtml(string htmlReport,
                                                                    string idParameterStart,
                                                                    string idParameterEnd)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            if (string.IsNullOrWhiteSpace(idParameterStart)
             || string.IsNullOrWhiteSpace(idParameterEnd))
                return parameters;

            var htmlPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), htmlReport);

            if (!File.Exists(htmlPath)) return parameters;

            string htmlFile = File.ReadAllText(htmlPath);

            parameters = GetValueBetween(htmlFile,
                                         idParameterStart,
                                         idParameterEnd);

            return parameters;
        }

        private static Dictionary<string, object> GetValueBetween(string stringText,
                                                                  string valueBeginsWith,
                                                                  string valueEndsWith)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            int strIndex = 0;
            do
            {
                var strPosIni = stringText.Substring(strIndex, stringText.Length - strIndex).IndexOf(valueBeginsWith);

                if (strPosIni == -1)
                    strIndex = stringText.Length;
                else
                {
                    var strPosFim = stringText.Substring(strIndex + strPosIni, stringText.Length - (strIndex + strPosIni)).IndexOf(valueEndsWith) + valueEndsWith.Length;

                    var parameter = stringText.Substring(strIndex + strPosIni, strPosFim);

                    if (!parameters.ContainsKey(parameter))
                        parameters.Add(parameter, null);

                    strIndex += (strPosIni + parameter.Length);
                }

            } while (strIndex < stringText.Length);

            return parameters;
        }

        public Dictionary<string, object> ExtractParametersFromHtml(string htmlReportPath,
                                                                    string idParameterStart,
                                                                    string idParameterEnd,
                                                                    object objClass)
        {
            var parameters = ExtractParametersFromHtml(htmlReportPath,
                                                       idParameterStart,
                                                       idParameterEnd);

            FillParameterValueFromObject(parameters,
                                         idParameterStart,
                                         idParameterEnd,
                                         objClass);

            return parameters;
        }

        public void FillParameterValueFromObject(Dictionary<string, object> parameters,
                                                 string idParameterStart,
                                                 string idParameterEnd,
                                                 object objClass)
        {

            if (parameters == null || parameters.Count == 0 || objClass == null) return;

            var paramCollection = parameters.Select(x => x.Key)
                                            .ToList();

            foreach (var parameter in paramCollection)
            {
                var nameOfProperty = parameter.Replace(idParameterStart, "").Replace(idParameterEnd, "");
                var propertyInfo = objClass.GetType().GetProperty(nameOfProperty);

                if (propertyInfo != null)
                {
                    parameters[parameter] = propertyInfo.GetValue(objClass, null);
                }
                else if (nameOfProperty.Contains("."))
                {
                    var childValue = GetPropValueFromChild(objClass, nameOfProperty);

                    if (childValue !=null)
                    {
                        parameters[parameter] = childValue;
                    }
                }

            }

        }

        private object GetPropValueFromChild(object objClass, string propName)
        {
            var nameOfObj = string.Join('.', propName.Split('.').Take(1));
            var nameOfProperty = string.Join('.', propName.Split('.').Skip(1));

            if (nameOfProperty.Contains("."))
            {
                return GetPropValueFromChild(objClass, nameOfProperty);
            }

            var childObj = objClass.GetType().GetProperty(nameOfObj)?.GetValue(objClass, null);
            return childObj?.GetType()
                           .GetProperty(nameOfProperty)?
                           .GetValue(childObj, null);
        }

        public byte[] GenerateQRCode(string encodeText,
                                     int qrCodeOutputFormat = 0,
                                     int width = 100,
                                     int height = 100)
        {
            byte[] qrCodeObject = null;

            switch (qrCodeOutputFormat)
            {
                case 0:

                    #region Renderer Pixel Data

                    var writerPixel = new BarcodeWriter<PixelData>()
                    {
                        Format = BarcodeFormat.QR_CODE,
                        Options = new QrCodeEncodingOptions()
                        {
                            DisableECI = true,
                            CharacterSet = "UTF-8",
                            Width = width,
                            Height = height
                        },
                        Renderer = new PixelDataRenderer()
                        {
                            Background = PixelDataRenderer.Color.White,
                            Foreground = PixelDataRenderer.Color.Black
                        },
                    };

                    var qrCodePixelData = writerPixel.Write(encodeText);

                    Bitmap bmp = new Bitmap(qrCodePixelData.Width, qrCodePixelData.Height);
                    Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
                    BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, bmp.PixelFormat);
                    IntPtr ptr = bmpData.Scan0;
                    Marshal.Copy(qrCodePixelData.Pixels, 0, ptr, qrCodePixelData.Pixels.Length);
                    bmp.UnlockBits(bmpData);

                    var mms = new MemoryStream();
                    bmp.Save(mms, ImageFormat.Bmp);

                    qrCodeObject = mms.GetBuffer();

                    #endregion

                    break;
                case 1:

                    #region Renderer SVG Data

                    var writerSvg = new BarcodeWriter<SvgImage>()
                    {
                        Format = BarcodeFormat.QR_CODE,
                        Options = new QrCodeEncodingOptions()
                        {
                            DisableECI = true,
                            CharacterSet = "UTF-8",
                            Width = width,
                            Height = height
                        },
                        Renderer = new SvgRenderer(),
                    };

                    var qrCodeSvgData = writerSvg.Write(encodeText);

                    qrCodeObject = Encoding.UTF8.GetBytes(qrCodeSvgData.Content);

                    #endregion

                    break;
            }

            return qrCodeObject;
        }

        #endregion

    }
}
