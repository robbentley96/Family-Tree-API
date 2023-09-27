using System;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

[assembly: FunctionsStartup(typeof(FamilyTreeAPI.Startup))]

namespace FamilyTreeAPI
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddDbContext<FamilyTreeContext>(options => options.UseSqlServer(Environment.GetEnvironmentVariable("FamilyTreeDatabase")));
            builder.Services.AddTransient<IPersonService, PersonService>();
        }
    }
}