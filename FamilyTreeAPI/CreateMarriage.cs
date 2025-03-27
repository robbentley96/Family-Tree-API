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
    public class CreateMarriage
    {
        private readonly IMarriageService _marriageService;
		private readonly ILogger _logger;
		public CreateMarriage(IMarriageService marriageService, ILoggerFactory loggerFactory)
        {
			_marriageService = marriageService;
            _logger = loggerFactory.CreateLogger<CreateMarriage>();
	}
        [Function("CreateMarriage")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] Marriage marriage)
        {
			_logger.LogInformation("C# HTTP trigger function processed a request.");

            await _marriageService.CreateMarriage(marriage);
            return new OkResult();
        }
    }
}
