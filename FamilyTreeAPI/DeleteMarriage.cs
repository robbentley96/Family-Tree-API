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
    public class DeleteMarriage
    {
        private readonly IMarriageService _marriageService;
		private readonly ILogger _logger;
		public DeleteMarriage(IMarriageService marriageService, ILoggerFactory loggerFactory)
        {
			_marriageService = marriageService;
			_logger = loggerFactory.CreateLogger<DeleteMarriage>();
		}
        [Function("DeleteMarriage")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "DeleteMarriage/{marriageID}")] HttpRequest req, string marriageID)
        {
			_logger.LogInformation("C# HTTP trigger function processed a request.");
            
            await _marriageService.DeleteMarriage(marriageID);
            return new OkResult();
        }
    }
}
