using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions;
using FunctionAppMoedas.Data;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureOpenApi()
    .ConfigureServices(services => {
        services.AddDbContext<MoedasContext>(
            options => options.UseSqlServer(
                Environment.GetEnvironmentVariable("BaseServerlessMoedas")!));
        services.AddScoped<CotacoesRepository>();
    })
    .Build();

host.Run();