using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ZooERP.Application;
using ZooERP.Core.Interfaces;
using ZooERP.Core.Services;

namespace ZooERP;

public static class Program
{
    public static void Main()
    {
        // Настройка DI-контейнера
        var host = Host.CreateDefaultBuilder().ConfigureServices(services =>
        {
            services.AddSingleton<IVetClinic, VetClinicService>();
            services.AddSingleton<IZoo, ZooService>();
        })
        .Build();
        // Создание и запуск приложения
        var zooService = host.Services.GetRequiredService<IZoo>();
        var app = new ZooApplication(zooService);

        app.Run();
    }
}