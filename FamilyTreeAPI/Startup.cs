using System;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore.Sqlite;
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
        }
    }
}