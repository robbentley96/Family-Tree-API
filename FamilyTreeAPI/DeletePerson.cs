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
    public class DeletePerson
    {
        private readonly IPersonService _personService;
        private readonly ILogger<DeletePerson> _logger;
        public DeletePerson(IPersonService personService, ILoggerFactory loggerFactory)
        {
            _personService = personService;
			_logger = loggerFactory.CreateLogger<DeletePerson>();
		}
        [Function("DeletePerson")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "DeletePerson/{personID}")] HttpRequest req, string personID)
        {
			_logger.LogInformation("C# HTTP trigger function processed a request.");
            
            await _personService.DeletePerson(personID);
            return new OkResult();
        }
    }
}
