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
    public class UpdateMarriage
    {
        private readonly IMarriageService _marriageService;
		private readonly ILogger _logger;
		public UpdateMarriage(IMarriageService marriageService, ILoggerFactory loggerFactory)
        {
			_marriageService = marriageService;
			_logger = loggerFactory.CreateLogger<UpdateMarriage>();
		}
        [Function("UpdateMarriage")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "UpdateMarriage/{marriageId}")] Marriage marriage,
            string marriageId)
        {
			_logger.LogInformation("C# HTTP trigger function processed a request.");

            await _marriageService.UpdateMarriage(marriage, marriageId);
            return new OkResult();
        }
    }
}
