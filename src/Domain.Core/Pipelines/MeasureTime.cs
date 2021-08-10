using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Domain.Core.Pipelines
{
    public class MeasureTime<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {

        #region Variables

        private readonly ILogger<GlobalActionLogger> _logger;
        private readonly IHostEnvironment _hostingEnviroment;

        #endregion

        #region Constructors

        public MeasureTime(ILogger<GlobalActionLogger> logger,
                           IHostEnvironment hostingEnviroment)
        {
            _logger = logger;
            _hostingEnviroment = hostingEnviroment;
        }

        #endregion

        #region Methods

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var stopWatch = Stopwatch.StartNew();
            var result = await next();
            var elapsed = stopWatch.Elapsed;

            // Mostrar o tempo de execução
            Debug.WriteLine($"Tempo de execução do request {typeof(TRequest).FullName}: {elapsed}ms");

            if (_hostingEnviroment.IsDevelopment())
            {
                _logger.LogInformation(1,$"Tempo de execução do request {typeof(TRequest).FullName}: {elapsed}ms");
            }            

            return result;
        }

        #endregion

    }
}
