using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Orleans.Configuration;

using Orleans.Runtime;
using Silo.Config;

try
{
    var host = await StartSiloAsync();
    Console.WriteLine("\n\n Press Enter to terminate...\n\n");
    Console.ReadLine();

    await host.StopAsync();

    return 0;
}
catch (Exception ex)
{
    Console.WriteLine(ex);
    return 1;
}

static async Task<IHost> StartSiloAsync()
{
    var config = new ConfigurationBuilder()
           .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
           .AddJsonFile("appsettings.json")
           .AddUserSecrets<Program>()
           .Build();


    var s = config.GetValue<string>("Settings:Test");

    var settings = config.GetSection(Settings.SettingsName).Get<Settings>();


    var builder = new HostBuilder()
        .UseOrleans(c =>
        {
            c.UseLocalhostClustering()
            .AddAzureTableGrainStorage(name: "SimpleTableStorage1", o =>
            {
                o.ConfigureTableServiceClient(settings.SimpleTableStorage);
            })
            .Configure<ClusterOptions>(options =>
            {
                options.ClusterId = "dev";
                options.ServiceId = "OrleansBasics";
            })
            .ConfigureLogging(logging => logging.AddConsole());
        });

    var host = builder.Build();
    await host.StartAsync();

    return host;
}