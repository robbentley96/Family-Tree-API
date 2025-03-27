using FamilyTreeAPI;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
	.ConfigureFunctionsWebApplication()
	.ConfigureServices(services =>
	{
		services.AddApplicationInsightsTelemetryWorkerService();
		services.ConfigureFunctionsApplicationInsights();
		services.AddScoped<IGoogleSheetsService, GoogleSheetsService>();
		services.AddScoped<IMarriageService, MarriageService>();
		services.AddScoped<IPersonService, PersonService>();
	})
	.Build();

host.Run();