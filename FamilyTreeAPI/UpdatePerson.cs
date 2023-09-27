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
    public class UpdatePerson
    {
        private readonly IPersonService _personService;
        public UpdatePerson(IPersonService personService)
        {
            _personService = personService;
        }
        [FunctionName("UpdatePerson")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "UpdatePerson/{personId}")] Person person,
            ILogger log, int personId)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            await _personService.UpdatePerson(person, personId);
            return new OkResult();
        }
    }
}
