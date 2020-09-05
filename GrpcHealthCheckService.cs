using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GrpcHealthCheck
{
    public class GrpcHealthCheckService : BackgroundService
    {
        private readonly Greeter.GreeterClient _client;
        public GrpcHealthCheckService(Greeter.GreeterClient client)
        {
            _client = client;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var reply = await _client.SayHelloAsync(new HelloRequest { Name = "GreeterClient" });
                Console.WriteLine(reply.Message);
                if (reply.Message == "Hello GreeterClient")
                    HealthModel.HealthStatus = "Healthy";
                else
                    HealthModel.HealthStatus = "Unhealthy";
                await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
            }
        }
    }
}