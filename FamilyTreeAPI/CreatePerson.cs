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
    public class CreatePerson
    {
        private readonly IPersonService _personService;
        public CreatePerson(IPersonService personService)
        {
            _personService = personService;
        }
        [FunctionName("CreatePerson")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] Person person,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            await _personService.CreatePerson(person);
            return new OkResult();
        }
    }
}
