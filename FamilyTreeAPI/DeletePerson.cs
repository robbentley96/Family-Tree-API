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
    public class DeletePerson
    {
        private readonly IPersonService _personService;
        public DeletePerson(IPersonService personService)
        {
            _personService = personService;
        }
        [FunctionName("DeletePerson")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "DeletePerson/{personID}")] HttpRequest req, int personID,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            
            await _personService.DeletePerson(personID);
            return new OkResult();
        }
    }
}
