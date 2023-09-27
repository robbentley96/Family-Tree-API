using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace FamilyTreeAPI
{
    public class GetPeople
    {
        private readonly IPersonService _personService;
        public GetPeople(IPersonService personService)
        {
            _personService = personService;
        }
        [FunctionName("GetPeople")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            List<Person> output = await _personService.GetPeople();
            return new OkObjectResult(output);
        }
    }
}
