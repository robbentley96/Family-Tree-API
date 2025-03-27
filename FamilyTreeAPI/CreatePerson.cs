using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.Functions.Worker;

namespace FamilyTreeAPI
{
    public class CreatePerson
    {
        private readonly IPersonService _personService;
        private readonly ILogger<CreatePerson> _logger;
        public CreatePerson(IPersonService personService, ILoggerFactory loggerFactory)
        {
            _personService = personService;
			_logger = loggerFactory.CreateLogger<CreatePerson>();
		}
        [Function("CreatePerson")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] Person person)
        {
			_logger.LogInformation("C# HTTP trigger function processed a request.");

            CreatePersonResponse response = await _personService.CreatePerson(person);
            return new OkObjectResult(response);
        }
    }
}
