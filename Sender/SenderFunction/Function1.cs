using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SenderLib;

namespace SenderFunction
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            try
            {
                var requestBody = await new StreamReader(req.Body).ReadToEndAsync();

                var sender = new Sender();
                await sender.RunAsync(requestBody);

                log.LogInformation("end.");

                return new OkObjectResult("OK!");
            }
            catch (Exception ex)
            {
                log.LogError(ex, "error end.");

                return new OkObjectResult("Error!");
            }
        }
    }
}
