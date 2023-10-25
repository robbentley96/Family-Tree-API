using System;
using Microsoft.AspNetCore.Hosting;
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
        public Startup(IHostingEnvironment env)
        {
            using(var client = new FamilyTreeContext())
            {
                client.Database.EnsureCreated();
            }
        }
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddDbContext<FamilyTreeContext>(options => options.UseSqlite("Data Source=../../../FamilyTree.db"));
            builder.Services.AddTransient<IPersonService, PersonService>();
        }
    }
}