using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FamilyTreeAPI
{
    public class GetPerson
    {
        private readonly IPersonService _personService;
        private readonly ILogger<GetPerson> _logger;
        public GetPerson(IPersonService personService, ILoggerFactory loggerFactory)
        {
            _personService = personService;
			_logger = loggerFactory.CreateLogger<GetPerson>();
		}
        [Function("GetPerson")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "GetPerson/{personID}")] HttpRequest req, string personID)
        {
			_logger.LogInformation("C# HTTP trigger function processed a request.");

            PersonDTO person = await _personService.GetPersonDTO(personID);
            return new OkObjectResult(person);
        }
    }
}
