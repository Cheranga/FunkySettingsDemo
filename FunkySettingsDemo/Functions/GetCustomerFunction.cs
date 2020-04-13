using System;
using System.Collections.Generic;
using System.Text;
using FunkySettingsDemo.Configurations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace FunkySettingsDemo.Functions
{
    public class GetCustomerFunction
    {
        private readonly DatabaseConfig _databaseConfig;
        private readonly OrdersApiConfig _ordersApiConfig;
        private readonly ILogger<GetCustomerFunction> _logger;

        public GetCustomerFunction(DatabaseConfig databaseConfig,
            OrdersApiConfig ordersApiConfig,
            ILogger<GetCustomerFunction> logger)
        {
            _databaseConfig = databaseConfig;
            _ordersApiConfig = ordersApiConfig;
            _logger = logger;
        }

        [FunctionName(nameof(GetCustomerFunction))]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "customers")]
            HttpRequest request)
        {
            return new OkResult();
        }
    }
}
