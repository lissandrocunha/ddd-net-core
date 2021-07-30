using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infra.CrossCutting.AspNetFilters
{
    public class GlobalActionLogger : IActionFilter, IOrderedFilter
    {

        #region Variables

        private readonly ILogger<GlobalExceptionHandlingFilter> _logger;
        private readonly IHostEnvironment _hostingEnviroment;

        #endregion

        #region Properties

        public int Order => int.MaxValue - 10;

        #endregion

        #region Constructors

        public GlobalActionLogger(ILogger<GlobalExceptionHandlingFilter> logger,
                                  IHostEnvironment hostingEnviroment)
        {
            _logger = logger;
            _hostingEnviroment = hostingEnviroment;
        }

        #endregion

        #region Methods

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (_hostingEnviroment.IsDevelopment() 
             || _hostingEnviroment.IsStaging())
            {
                var data = new
                {
                    Version = "v1.0",
                    User = context.HttpContext.User.Identity.Name,
                    IP = context.HttpContext.Connection.RemoteIpAddress.ToString(),
                    Hostname = context.HttpContext.Request.Host.ToString(),
                    AreaAccessed = context.HttpContext.Request.GetDisplayUrl(),
                    Action = context.ActionDescriptor.DisplayName,
                    TimeStamp = DateTime.Now
                };

                _logger.LogInformation(1, data.ToString());
            }

            if (_hostingEnviroment.IsProduction())
            {
                
            }

        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            
        }

        #endregion

    }
}