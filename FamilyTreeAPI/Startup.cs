using System;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
[assembly: FunctionsStartup(typeof(FamilyTreeAPI.Startup))]

namespace FamilyTreeAPI
{
    public class Startup : FunctionsStartup
    {
        public Startup()
        {

        }
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddTransient<IPersonService, PersonService>();
            builder.Services.AddTransient<IGoogleSheetsService, GoogleSheetsService>();
        }
    }
}