using FunkySettingsDemo;
using FunkySettingsDemo.Configurations;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

[assembly: FunctionsStartup(typeof(Startup))]

namespace FunkySettingsDemo
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var services = builder.Services;
            //
            // Get the function app directory
            //
            var executionContextOptions = builder.Services.BuildServiceProvider().GetService<IOptions<ExecutionContextOptions>>().Value;
            var currentDirectory = executionContextOptions.AppDirectory;
            //
            // Load the custom configuration files
            //
            var config = new ConfigurationBuilder()
                .SetBasePath(currentDirectory)
                .AddJsonFile("databaseconfig.json", false)
                .AddJsonFile("ordersapiconfig.json", false)
                .AddEnvironmentVariables()
                .Build();
            //
            // Need to register the above setup configuration provider which includes the custom configuration files
            //
            services.AddSingleton<IConfiguration>(config);

            services.AddOptions<DatabaseConfig>().Configure<IConfiguration>((customSetting, configuration) => { configuration.GetSection("databaseconfig").Bind(customSetting); });

            services.AddOptions<OrdersApiConfig>().Configure<IConfiguration>((customSetting, configuration) => { configuration.GetSection("ordersapiconfig").Bind(customSetting); });


            services.AddScoped(provider =>
            {
                var options = provider.GetRequiredService<IOptions<DatabaseConfig>>();
                return options.Value;
            });

            services.AddScoped(provider =>
            {
                var options = provider.GetRequiredService<IOptions<OrdersApiConfig>>();
                return options.Value;
            });
        }
    }
}