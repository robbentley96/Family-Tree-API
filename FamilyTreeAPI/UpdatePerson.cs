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
    public class UpdatePerson
    {
        private readonly IPersonService _personService;
		private readonly ILogger _logger;
		public UpdatePerson(IPersonService personService, ILoggerFactory loggerFactory)
        {
            _personService = personService;
			_logger = loggerFactory.CreateLogger<UpdatePerson>();
		}
        [Function("UpdatePerson")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "UpdatePerson/{personId}")] Person person,
            string personId)
        {
			_logger.LogInformation("C# HTTP trigger function processed a request.");

            await _personService.UpdatePerson(person, personId);
            return new OkResult();
        }
    }
}
