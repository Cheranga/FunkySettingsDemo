using System;
using FunkySettingsDemo;
using FunkySettingsDemo.Configurations;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

[assembly: FunctionsStartup(typeof(Startup))]

namespace FunkySettingsDemo
{
    public class CustomStartup : IWebJobsStartup
    {
        public void Configure(IWebJobsBuilder builder)
        {
            
        }
    }

    public class Startup : FunctionsStartup
    {
        

        public override void Configure(IFunctionsHostBuilder builder)
        {
            var services = builder.Services;
            //
            // TODO: register dependencies
            //

            
            var config = new ConfigurationBuilder()
                .SetBasePath(Environment.CurrentDirectory)
                .AddJsonFile("databaseconfig.json", false)
                .AddJsonFile("ordersapiconfig.json", false)
                .AddEnvironmentVariables()
                .Build();
           

            services.AddSingleton<IConfiguration>(config);

            services.AddOptions<DatabaseConfig>().Configure<IConfiguration>((customSetting, configuration) =>
            {
                configuration.GetSection("DatabaseConfig").Bind(customSetting);
            });

            services.AddOptions<OrdersApiConfig>().Configure<IConfiguration>((customSetting, configuration) =>
            {
                configuration.GetSection("OrdersApiConfig").Bind(customSetting);
            });


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