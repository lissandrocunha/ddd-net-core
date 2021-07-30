using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;

namespace Infra.CrossCutting.AspNetFilters
{
    public class GlobalExceptionHandlingFilter : IExceptionFilter
    {

        #region Variables

        private readonly ILogger<GlobalExceptionHandlingFilter> _logger;
        private readonly IHostEnvironment _hostingEnviroment;

        #endregion

        #region Constructors

        public GlobalExceptionHandlingFilter(ILogger<GlobalExceptionHandlingFilter> logger,
                                             IHostEnvironment hostingEnviroment)
        {
            _logger = logger;
            _hostingEnviroment = hostingEnviroment;
        }

        #endregion

        #region Methods

        public void OnException(ExceptionContext context)
        {
            string exceptionMessage = context.Exception.Message;

            if (_hostingEnviroment.IsProduction())
            {
                _logger.LogError(1, context.Exception, exceptionMessage);
                exceptionMessage = "Ocorreu um erro interno no servidor.";
            }

            if (_hostingEnviroment.IsDevelopment())
            {
                
            }

            var result = new ViewResult { ViewName = "Error" };
            var modelData = new EmptyModelMetadataProvider();
            result.ViewData = new ViewDataDictionary(modelData, context.ModelState)
            {
                {"MessageErro",exceptionMessage }
            };

            context.ExceptionHandled = true;
            context.Result = result;
        }

        #endregion

    }
}
