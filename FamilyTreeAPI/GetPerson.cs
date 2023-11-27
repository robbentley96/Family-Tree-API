using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FamilyTreeAPI
{
    public class GetPerson
    {
        private readonly IPersonService _personService;
        public GetPerson(IPersonService personService)
        {
            _personService = personService;
        }
        [FunctionName("GetPerson")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "GetPerson/{personID}")] HttpRequest req, string personID, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            Person person = await _personService.GetPerson(personID);
            return new OkObjectResult(person);
        }
    }
}
