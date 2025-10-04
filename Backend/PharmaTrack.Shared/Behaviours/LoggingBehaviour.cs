using System;

namespace PharmaTrack.Shared.Behaviours;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class LoggingBehaviour<TRequest, TResponse>(ILogger<LoggingBehaviour<TRequest, TResponse>> logger) :
        IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull,IRequest<TResponse>
        where TResponse : notnull
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            logger.LogInformation("[Start] handle request ={Request} - Response ={Response} -Request Data={RequestData}",
                typeof(TRequest).Name, typeof(TResponse).Name, request);
            var timer= new Stopwatch();
            timer.Start();

            var response = await next();
            timer.Stop();
            var timetaken = timer.Elapsed;
            if (timetaken .Seconds> 3)
            {
                logger.LogWarning("[Performance] handle request ={Request} - Response ={Response} -Request Data={RequestData} -TimeTaken={TimeTaken}",
                    typeof(TRequest).Name, typeof(TResponse).Name, request, timetaken.Seconds);
            }
            else
            {
                logger.LogInformation("[End] handle request ={Request} - Response ={Response} -Request Data={RequestData} -TimeTaken={TimeTaken}",
                    typeof(TRequest).Name, typeof(TResponse).Name, request, timetaken.Seconds);

            }


            return response;
        }
    
}
