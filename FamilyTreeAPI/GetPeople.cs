using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace FamilyTreeAPI
{
    public class GetPeople
    {
        private readonly IPersonService _personService;
        private readonly ILogger<GetPeople> _logger;
        public GetPeople(IPersonService personService, ILoggerFactory loggerFactory)
        {
            _personService = personService;
			_logger = loggerFactory.CreateLogger<GetPeople>();
		}
        [Function("GetPeople")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req)
        {
			_logger.LogInformation("C# HTTP trigger function processed a request.");

            List<PersonSimplifiedDTO> output = await _personService.GetPeople();
            return new OkObjectResult(output);
        }
    }
}
