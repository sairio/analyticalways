using Business;
using Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Analyticalways
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var config = LoadConfiguration();

            var builder = new HostBuilder()
               .ConfigureServices((hostContext, services) =>
               {
                   services
                   //.AddLogging(configure => configure.AddConsole())
                   .AddTransient<StockApplication>()
                   .AddScoped<IRemoteDownloadServices, RemoteDownloadServices>()
                   .AddSingleton<IStockService, StockService>()
                   .AddSingleton(config)
                   .AddDbContext<ApplicationContext>(options =>
                   {
                       options.UseSqlServer(config.GetConnectionString("DbConnection"));
                   });
               }).UseConsoleLifetime();

            var host = builder.Build();

            using (IServiceScope serviceScope = host.Services.CreateScope())
            {
                var services = serviceScope.ServiceProvider;

                try
                {
                    StockApplication myService = services.GetRequiredService<StockApplication>();
                    await myService.Run();

                    Console.WriteLine("Success");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ocurrio un error Inesperado");
                }
            }
        }

        public static IConfiguration LoadConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            return builder.Build();
        }
    }
}
